using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extentions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.FileProvider.Convertors;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Messaging;
using PetFamily.Application.Providers;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.UploadPetPhoto;

public class UploadPetPhotosHandler(
    IVolunteersRepository volunteersRepository,
    IUnitOfWork unitOfWork,
    IFilesProvider filesProvider,
    IFileCleanerQueue fileCleanerQueue,
    IValidator<UploadPetPhotosCommand> validator,
    ILogger<UploadPetPhotosHandler> logger)
    : ICommandHandler<IEnumerable<string>, UploadPetPhotosCommand>
{
    public async Task<Result<IEnumerable<string>, ErrorList>> HandleAsync(
        UploadPetPhotosCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerResult = await volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return Errors.General.NotFound(command.VolunteerId).ToErrorList();
        
        var petResult = volunteerResult.Value.GetPetById(command.PetId);
        if (petResult.IsFailure)
            return Errors.General.NotFound(command.PetId).ToErrorList();
        
        var photosToUpload = command.Photos.Select(
            x => new FileData(x.Content, FileNameHelpers.GetRandomizedFileName(x.FileName))).ToList();;
        
        List<Photo> photos = [];
        foreach (var uploadPhoto in photosToUpload) 
        {
            var photo = Photo.Create(uploadPhoto.ObjectName);
            if (photo.IsFailure)
                return Errors.General.UploadFailure(photo.Error.ToString()).ToErrorList();
            
            photos.Add(photo.Value);
        }
        
        var uploadResult = await filesProvider.UploadFiles(photosToUpload, Constants.BUCKET_NAME, cancellationToken);
        if (uploadResult.IsFailure)
        {
            await fileCleanerQueue.PublishAsync(photosToUpload.Select(p => p.ObjectName), cancellationToken);
            return Errors.General.UploadFailure(uploadResult.Error.ToString()).ToErrorList();
        }

        petResult.Value.AddPhotos(photos);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        logger.LogInformation("Photos uploaded for pet with id {PetId}", petResult.Value.Id);

        return uploadResult.Value.ToList();
    }
}
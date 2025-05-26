using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extentions;
using PetFamily.Application.Features.Volunteers.UploadPetPhoto;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Providers;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.DeletePetPhoto;

public class DeletePetPhotosHandler(
    IVolunteersRepository volunteersRepository,
    IUnitOfWork unitOfWork,
    IFilesProvider filesProvider,
    IValidator<DeletePetPhotosCommand> validator,
    ILogger<UploadPetPhotosHandler> logger)
    : ICommandHandler<IEnumerable<string>, DeletePetPhotosCommand>
{
    public async Task<Result<IEnumerable<string>, ErrorList>> HandleAsync(
        DeletePetPhotosCommand command,
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

        var deleteResult = await filesProvider.DeleteFiles(
            command.PhotoNames,
            Constants.BUCKET_NAME,
            cancellationToken);

        if (deleteResult.IsFailure)
            return Errors.General.DeleteFileFailure(deleteResult.Error.ToString()).ToErrorList();

        List<Photo> photos = [];
        foreach (var photoName in command.PhotoNames)
        {
            var photo = Photo.Create(photoName);
        
            photos.Add(photo.Value);
        }

        petResult.Value.RemovePhotos(photos);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        logger.LogInformation("Photos deleted for pet with id {PetId}", petResult.Value.Id);

        return command.PhotoNames.ToList();
    }
    
}
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.FileProvider;
using PetFamily.Application.FileProvider.Convertors;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Messaging;
using PetFamily.Application.Providers;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Commands.UploadPetPhoto;

public class UploadPetPhotosHandler : ICommandHandler<UploadPetPhotosCommand, IEnumerable<string>>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFilesProvider _filesProvider;
    private readonly IFileCleanerQueue _fileCleanerQueue;
    private readonly ILogger<UploadPetPhotosHandler> _logger;

    public UploadPetPhotosHandler(
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork,
        IFilesProvider filesProvider,
        IFileCleanerQueue fileCleanerQueue,
        IValidator<UploadPetPhotosCommand> validator,
        ILogger<UploadPetPhotosHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _filesProvider = filesProvider;
        _fileCleanerQueue = fileCleanerQueue;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<string>, ErrorList>> Handle(
        UploadPetPhotosCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return Errors.General.NotFound(command.VolunteerId).ToErrorList();

        var petResult = volunteerResult.Value.GetPetById(command.PetId);
        if (petResult.IsFailure)
            return Errors.General.NotFound(command.PetId).ToErrorList();

        var photosToUpload = command.Photos
            .Select(x => new FileData(x.Content, FileNameHelpers.GetRandomizedFileName(x.FileName))).ToList();
        ;

        List<Photo> photos = [];
        foreach (var uploadPhoto in photosToUpload)
        {
            var photo = Photo.Create(uploadPhoto.ObjectName);
            if (photo.IsFailure)
                return Errors.General.UploadFailure(photo.Error.ToString()).ToErrorList();

            photos.Add(photo.Value);
        }

        var uploadResult = await _filesProvider.UploadFiles(photosToUpload, Constants.BUCKET_NAME, cancellationToken);
        if (uploadResult.IsFailure)
        {
            await _fileCleanerQueue.PublishAsync(photosToUpload.Select(p => p.ObjectName), cancellationToken);
            return Errors.General.UploadFailure(uploadResult.Error.ToString()).ToErrorList();
        }

        petResult.Value.AddPhotos(photos);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Photos uploaded for pet with id {PetId}", petResult.Value.Id);

        return uploadResult.Value.ToList();
    }
}
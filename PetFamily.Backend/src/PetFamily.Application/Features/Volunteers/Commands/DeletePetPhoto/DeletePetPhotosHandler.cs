using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Features.Volunteers.Commands.UploadPetPhoto;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Providers;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Commands.DeletePetPhoto;

public class DeletePetPhotosHandler : ICommandHandler<DeletePetPhotosCommand, IEnumerable<string>>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFilesProvider _filesProvider;
    private readonly ILogger<UploadPetPhotosHandler> _logger;

    public DeletePetPhotosHandler(
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork,
        IFilesProvider filesProvider,
        ILogger<UploadPetPhotosHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _filesProvider = filesProvider;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<string>, ErrorList>> Handle(
        DeletePetPhotosCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return Errors.General.NotFound(command.VolunteerId).ToErrorList();

        var petResult = volunteerResult.Value.GetPetById(command.PetId);
        if (petResult.IsFailure)
            return Errors.General.NotFound(command.PetId).ToErrorList();

        List<Photo> photos = [];
        foreach (var photoName in command.PhotoNames)
        {
            var photo = Photo.Create(photoName);
            photos.Add(photo.Value);
        }

        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            petResult.Value.RemovePhotos(photos);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var deleteResult = await _filesProvider.DeleteFiles(
                command.PhotoNames,
                Constants.BUCKET_NAME,
                cancellationToken);

            if (deleteResult.IsFailure)
            {
                transaction.Rollback();
                _logger.LogError("Failed to delete files for pet with id {PetId}. Transaction rolled back.", petResult.Value.Id);
                return Errors.General.DeleteFileFailure(deleteResult.Error.ToString()).ToErrorList();
            }

            transaction.Commit();

            _logger.LogInformation("Photos deleted for pet with id {PetId}", petResult.Value.Id);

            return command.PhotoNames.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting photos for pet with id {PetId}. Transaction rolled back.", petResult.Value.Id);
            transaction.Rollback();
            throw;
        }
    }
}
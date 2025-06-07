using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Features.Volunteers.Commands.UploadPetPhoto;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Providers;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Commands.DeletePetPhoto;

public class DeletePetPhotosHandler : ICommandHandler<IEnumerable<string>, DeletePetPhotosCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFilesProvider _filesProvider;
    private readonly IValidator<DeletePetPhotosCommand> _validator;
    private readonly ILogger<UploadPetPhotosHandler> _logger;

    public DeletePetPhotosHandler(
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork,
        IFilesProvider filesProvider,
        IValidator<DeletePetPhotosCommand> validator,
        ILogger<UploadPetPhotosHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _filesProvider = filesProvider;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<string>, ErrorList>> HandleAsync(
        DeletePetPhotosCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return Errors.General.NotFound(command.VolunteerId).ToErrorList();

        var petResult = volunteerResult.Value.GetPetById(command.PetId);
        if (petResult.IsFailure)
            return Errors.General.NotFound(command.PetId).ToErrorList();

        var deleteResult = await _filesProvider.DeleteFiles(
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

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Photos deleted for pet with id {PetId}", petResult.Value.Id);

        return command.PhotoNames.ToList();
    }
}
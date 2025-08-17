using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.FileProvider.Convertors;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Messaging;
using PetFamily.Application.Providers;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Commands.UploadPetPhoto;

public class UploadPetPhotosHandler : ICommandHandler<IEnumerable<string>, UploadPetPhotosCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFilesProvider _filesProvider;
    private readonly IFileCleanerQueue _fileCleanerQueue;
    private readonly IValidator<UploadPetPhotosCommand> _validator;
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
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<string>, ErrorList>> HandleAsync(
        UploadPetPhotosCommand command,
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
        
        var photosToUpload = command.Photos.Select(
            x => new FileData(x.Content, FileNameHelpers.GetRandomizedFileName(x.FileName))).ToList();
        
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
        
        System.Data.IDbTransaction transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            petResult.Value.AddPhotos(photos);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, "Error while uploading photos for pet with id {PetId}. Transaction Rollback", petResult.Value.Id);
            
            transaction.Rollback();
            
            await _fileCleanerQueue.PublishAsync(photosToUpload.Select(p => p.ObjectName), cancellationToken);
            return Errors.General.UploadFailure(uploadResult.Error.ToString()).ToErrorList();
        }
        
        _logger.LogInformation("Photos uploaded for pet with id {PetId}", petResult.Value.Id);

        return uploadResult.Value.ToList();
    }
}
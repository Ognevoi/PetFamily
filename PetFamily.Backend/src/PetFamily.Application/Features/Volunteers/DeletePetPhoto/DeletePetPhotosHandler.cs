using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Features.Volunteers.UploadPetPhoto;
using PetFamily.Application.Providers;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.DeletePetPhoto;

public class DeletePetPhotosHandler
{
    private const string BUCKET_NAME = "photos";

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

    public async Task<Result<IEnumerable<string>, Error>> Handle(
        DeletePetPhotosRequest request,
        CancellationToken cancellationToken = default)
    {
        
        var volunteerResult = await _volunteersRepository.GetById(request.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return Errors.General.NotFound(request.VolunteerId);
        
        var petResult = volunteerResult.Value.GetPetById(request.PetId);
        if (petResult.IsFailure)
            return Errors.General.NotFound(request.PetId);

        // var photosToDelete = request.PhotosName.Select(
        //         x => new FileData(x.Content, FileNameHelpers.GetRandomizedFileName(x.FileName)));

        var deleteResult = await _filesProvider.DeleteFiles(
            request.PhotoNames,
            BUCKET_NAME,
            cancellationToken);

        // if (deleteResult.IsFailure)
        //     return deleteResult.Error.ToList()

        List<Photo> photos = [];
        foreach (var photoName in request.PhotoNames)
        {
            var photo = Photo.Create(photoName);
        
            photos.Add(photo.Value);
        }

        petResult.Value.RemovePhotos(photos);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Photos deleted for pet with id {PetId}", petResult.Value.Id);

        return request.PhotoNames.ToList();
    }
}
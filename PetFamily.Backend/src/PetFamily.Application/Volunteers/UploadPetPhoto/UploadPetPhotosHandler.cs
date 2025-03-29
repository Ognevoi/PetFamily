using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.FileProvider;
using PetFamily.Application.FileProvider.Convertors;
using PetFamily.Application.Providers;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UploadPetPhoto;

public class UploadPetPhotosHandler
{
    private const string BUCKET_NAME = "photos";

    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFilesProvider _filesProvider;
    private readonly ILogger<UploadPetPhotosHandler> _logger;
    
    public UploadPetPhotosHandler(
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
        UploadPetPhotosRequest request,
        CancellationToken cancellationToken = default)
    {
        
        var volunteerResult = await _volunteersRepository.GetById(request.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return Errors.General.NotFound(request.VolunteerId);
        
        var petResult = volunteerResult.Value.GetPetById(request.PetId);
        if (petResult.IsFailure)
            return Errors.General.NotFound(request.PetId);

        var photosToUpload = request.Photos.Select(
                x => new FileData(x.Content, FileNameHelpers.GetRandomizedFileName(x.FileName)));

        var uploadResult = await _filesProvider.UploadFiles(
            photosToUpload,
            BUCKET_NAME,
            cancellationToken);

        List<Photo> photos = [];
        foreach (var uploadPhoto in uploadResult.Value)
        {
            var photo = Photo.Create(uploadPhoto);

            photos.Add(photo.Value);
        }

        petResult.Value.AddPhotos(photos);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Photos uploaded for pet with id {PetId}", petResult.Value.Id);

        return uploadResult.Value.ToList();
    }
}
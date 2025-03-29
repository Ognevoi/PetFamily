using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.GetPetPhoto;

public class GetPetPhotosHandler
{
    private const string BUCKET_NAME = "photos";

    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IFilesProvider _filesProvider;
    private readonly ILogger<GetPetPhotosHandler> _logger;
    
    public GetPetPhotosHandler(
        IVolunteersRepository volunteersRepository,
        IFilesProvider filesProvider,
        ILogger<GetPetPhotosHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _filesProvider = filesProvider;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<string>, Error>> Handle(
        GetPetPhotosRequest request,
        CancellationToken cancellationToken = default)
    {
        
        var volunteerResult = await _volunteersRepository.GetById(request.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return Errors.General.NotFound(request.VolunteerId);
        
        var petResult = volunteerResult.Value.GetPetById(request.PetId);
        if (petResult.IsFailure)
            return Errors.General.NotFound(request.PetId);

        var photosToGet = petResult.Value.Photos.Select(x => x.FileName);

        var getResult = await _filesProvider.GetFileLink(
            photosToGet,
            BUCKET_NAME,
            cancellationToken);

        _logger.LogInformation("Photos for pet with id {PetId} retrieved", request.PetId);
        
        return getResult.Value.ToList();
    }
}
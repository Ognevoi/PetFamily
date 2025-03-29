using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Species.CreateSpecie;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.DeleteBreed;

public class DeleteBreedHandler
{
    private readonly ISpeciesRepository _specieRepository;
    private readonly ILogger<CreateSpecieHandler> _logger;

    public DeleteBreedHandler(
        ISpeciesRepository specieRepository,
        ILogger<CreateSpecieHandler> logger
        )
    {
        _specieRepository = specieRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        DeleteBreedRequest request,
        CancellationToken cancellationToken = default)
    {
        var specieResult = await _specieRepository.GetById(request.SpecieId, cancellationToken);
        if (specieResult.IsFailure)
            return specieResult.Error;

        var breedResult = specieResult.Value.GetBreed(request.BreedId);
        if (breedResult.IsFailure)
            return breedResult.Error;
     
        specieResult.Value.RemoveBreed(breedResult.Value);
        
        await _specieRepository.Save(specieResult.Value, cancellationToken);

        _logger.LogInformation("Delete breed with id: {BreedId}", breedResult.Value.Id.Value);

        return breedResult.Value.Id.Value;
    }
    
}
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Species.CreateSpecie;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.SpecieManagement.Entities;
using PetFamily.Domain.SpecieManagement.Value_Objects;

namespace PetFamily.Application.Species.AddBreed;

public class AddBreedHandler
{
    private readonly ISpeciesRepository _specieRepository;
    private readonly ILogger<CreateSpecieHandler> _logger;

    public AddBreedHandler(
        ISpeciesRepository specieRepository,
        ILogger<CreateSpecieHandler> logger
        )
    {
        _specieRepository = specieRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        AddBreedRequest request,
        CancellationToken cancellationToken = default)
    {
        var specieResult = await _specieRepository.GetById(request.SpecieId, cancellationToken);
        
        if (specieResult.IsFailure)
            return specieResult.Error;
        
        var nameResult = Name.Create(request.Dto.Name).Value;
        
        var breedId = BreedId.NewBreedId();

        var breedToCreate = new Breed(breedId, nameResult.Value);
        
        specieResult.Value.AddBreed(breedToCreate);
        
        var result = await _specieRepository.Save(specieResult.Value, cancellationToken);

        _logger.LogInformation("Create breed with id: {BreedId}", breedToCreate.Id);

        return result;
    }
    
}
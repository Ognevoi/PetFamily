using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Features.Species.CreateSpecie;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.SpecieManagement.Entities;
using PetFamily.Domain.SpecieManagement.Value_Objects;

namespace PetFamily.Application.Features.Species.AddBreed;

public class AddBreedHandler(
    ISpeciesRepository specieRepository,
    ILogger<CreateSpecieHandler> logger)
    : ICommandHandler<Guid, AddBreedCommand>
{
    public async Task<Result<Guid, ErrorList>> HandleAsync(
        AddBreedCommand command,
        CancellationToken cancellationToken = default)
    {
        var specieResult = await specieRepository.GetById(command.SpecieId, cancellationToken);
        
        if (specieResult.IsFailure)
            return specieResult.Error.ToErrorList();
        
        var nameResult = Name.Create(command.Name).Value;
        
        var breedId = BreedId.NewBreedId();

        var breedToCreate = new Breed(breedId, nameResult.Value);
        
        specieResult.Value.AddBreed(breedToCreate);
        
        var result = await specieRepository.Save(specieResult.Value, cancellationToken);

        logger.LogInformation("Create breed with id: {BreedId}", breedToCreate.Id);

        return result;
    }
    
}
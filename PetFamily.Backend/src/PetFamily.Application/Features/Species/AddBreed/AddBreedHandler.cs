using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Features.Species.CreateSpecie;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.SpecieManagement.Entities;
using PetFamily.Domain.SpecieManagement.Value_Objects;

namespace PetFamily.Application.Features.Species.AddBreed;

public class AddBreedHandler : ICommandHandler<AddBreedCommand, Guid>
{
    private readonly ILogger<CreateSpecieHandler> _logger;
    private readonly ISpeciesRepository _specieRepository;

    public AddBreedHandler(
        ISpeciesRepository specieRepository,
        ILogger<CreateSpecieHandler> logger)
    {
        _specieRepository = specieRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddBreedCommand command,
        CancellationToken cancellationToken = default)
    {
        var specieResult = await _specieRepository.GetById(command.SpecieId, cancellationToken);

        if (specieResult.IsFailure)
            return specieResult.Error.ToErrorList();

        var nameResult = Name.Create(command.Name).Value;

        var breedId = BreedId.NewBreedId();

        var breedToCreate = new Breed(breedId, nameResult.Value);

        specieResult.Value.AddBreed(breedToCreate);

        var result = await _specieRepository.Save(specieResult.Value, cancellationToken);

        _logger.LogInformation("Create breed with id: {BreedId}", breedToCreate.Id);

        return result;
    }
}
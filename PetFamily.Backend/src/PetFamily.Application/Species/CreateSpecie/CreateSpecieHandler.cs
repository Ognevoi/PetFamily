using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.SpecieManagement.AggregateRoot;
using PetFamily.Domain.SpecieManagement.Value_Objects;

namespace PetFamily.Application.Species.CreateSpecie;

public class CreateSpecieHandler
{
    private readonly ISpeciesRepository _specieRepository;
    private readonly ILogger<CreateSpecieHandler> _logger;

    public CreateSpecieHandler(
        ISpeciesRepository specieRepository,
        ILogger<CreateSpecieHandler> logger
        )
    {
        _specieRepository = specieRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        CreateSpecieRequest request,
        CancellationToken cancellationToken = default)
    {
        var specieId = SpecieId.NewSpecieId();
        
        var nameResult = Name.Create(request.Name).Value;

        var specieToCreate = Specie.Create(specieId, nameResult.Value);
        
        await _specieRepository.Add(specieToCreate.Value, cancellationToken);

        _logger.LogInformation("Create specie with id: {SpecieId}", specieToCreate.Value.Id);

        return (Guid)specieToCreate.Value.Id;
    }
    
}
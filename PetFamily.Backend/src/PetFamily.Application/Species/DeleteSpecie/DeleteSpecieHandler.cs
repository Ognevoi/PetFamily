using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Species.CreateSpecie;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.DeleteSpecie;

public class DeleteSpecieHandler
{
    private readonly ISpeciesRepository _specieRepository;
    private readonly ILogger<CreateSpecieHandler> _logger;

    public DeleteSpecieHandler(
        ISpeciesRepository specieRepository,
        ILogger<CreateSpecieHandler> logger
        )
    {
        _specieRepository = specieRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        DeleteSpecieRequest request,
        CancellationToken cancellationToken = default)
    {
        var specieResult = await _specieRepository.GetById(request.SpecieId);
        if (specieResult.IsFailure)
            return specieResult.Error;
        
        await _specieRepository.Delete(specieResult.Value, cancellationToken);
        
        await _specieRepository.Save(specieResult.Value, cancellationToken);

        _logger.LogInformation("Delete specie with id: {SpecieId}", specieResult.Value.Id);

        return specieResult.Value.Id.Value;
    }
    
}
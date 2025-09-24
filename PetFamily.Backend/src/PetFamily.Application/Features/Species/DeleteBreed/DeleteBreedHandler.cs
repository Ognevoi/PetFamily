using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Species.DeleteBreed;

public class DeleteBreedHandler : ICommandHandler<DeleteBreedCommand, Guid>
{
    private readonly ISpeciesRepository _specieRepository;
    private readonly ILogger<DeleteBreedHandler> _logger;

    public DeleteBreedHandler(
        ISpeciesRepository specieRepository,
        ILogger<DeleteBreedHandler> logger)
    {
        _specieRepository = specieRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteBreedCommand command,
        CancellationToken cancellationToken = default)
    {
        var specieResult = await _specieRepository.GetById(command.SpecieId, cancellationToken);
        if (specieResult.IsFailure)
            return specieResult.Error.ToErrorList();

        var breedResult = specieResult.Value.GetBreed(command.BreedId);
        if (breedResult.IsFailure)
            return breedResult.Error.ToErrorList();

        specieResult.Value.RemoveBreed(breedResult.Value);

        await _specieRepository.Save(specieResult.Value, cancellationToken);

        _logger.LogInformation("Delete breed with id: {BreedId}", breedResult.Value.Id.Value);

        return breedResult.Value.Id.Value;
    }
}
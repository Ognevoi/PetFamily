using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Features.Species.CreateSpecie;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Species.DeleteSpecie;

public class DeleteSpecieHandler : ICommandHandler<DeleteSpecieCommand, Guid>
{
    private readonly ISpeciesRepository _specieRepository;
    private readonly ILogger<CreateSpecieHandler> _logger;

    public DeleteSpecieHandler(
        ISpeciesRepository specieRepository,
        ILogger<CreateSpecieHandler> logger)
    {
        _specieRepository = specieRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteSpecieCommand command,
        CancellationToken cancellationToken = default)
    {
        var specieResult = await _specieRepository.GetById(command.SpecieId);
        if (specieResult.IsFailure)
            return specieResult.Error.ToErrorList();

        await _specieRepository.Delete(specieResult.Value, cancellationToken);

        await _specieRepository.Save(specieResult.Value, cancellationToken);

        _logger.LogInformation("Delete specie with id: {SpecieId}", specieResult.Value.Id);

        return specieResult.Value.Id.Value;
    }
}
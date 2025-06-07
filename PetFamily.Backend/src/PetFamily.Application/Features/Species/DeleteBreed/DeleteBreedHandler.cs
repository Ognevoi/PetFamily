using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.Features.Species.CreateSpecie;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Species.DeleteBreed;

public class DeleteBreedHandler : ICommandHandler<Guid, DeleteBreedCommand>
{
    private readonly ISpeciesRepository _specieRepository;
    private readonly IValidator<DeleteBreedCommand> _validator;
    private readonly ILogger<CreateSpecieHandler> _logger;

    public DeleteBreedHandler(
        ISpeciesRepository specieRepository,
        IValidator<DeleteBreedCommand> validator,
        ILogger<CreateSpecieHandler> logger)
    {
        _specieRepository = specieRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        DeleteBreedCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

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
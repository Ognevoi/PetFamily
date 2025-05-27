using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extentions;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.SpecieManagement.AggregateRoot;
using PetFamily.Domain.SpecieManagement.Value_Objects;

namespace PetFamily.Application.Features.Species.CreateSpecie;

public class CreateSpecieHandler : ICommandHandler<Guid, CreateSpecieCommand>
{
    private readonly ISpeciesRepository _specieRepository;
    private readonly IValidator<CreateSpecieCommand> _validator;
    private readonly ILogger<CreateSpecieHandler> _logger;

    public CreateSpecieHandler(
        ISpeciesRepository specieRepository,
        IValidator<CreateSpecieCommand> validator,
        ILogger<CreateSpecieHandler> logger)
    {
        _specieRepository = specieRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        CreateSpecieCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var specieId = SpecieId.NewSpecieId();

        var nameResult = Name.Create(command.Name).Value;

        var specieToCreate = Specie.Create(specieId, nameResult.Value);

        await _specieRepository.Add(specieToCreate.Value, cancellationToken);

        _logger.LogInformation("Create specie with id: {SpecieId}", specieToCreate.Value.Id);

        return (Guid)specieToCreate.Value.Id;
    }
}
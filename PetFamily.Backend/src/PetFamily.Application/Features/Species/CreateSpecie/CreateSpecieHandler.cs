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

public class CreateSpecieHandler(
    ISpeciesRepository specieRepository,
    IValidator<CreateSpecieCommand> validator,
    ILogger<CreateSpecieHandler> logger)
    : ICommandHandler<Guid, CreateSpecieCommand>
{
    public async Task<Result<Guid, ErrorList>> HandleAsync(
        CreateSpecieCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var specieId = SpecieId.NewSpecieId();

        var nameResult = Name.Create(command.Name).Value;

        var specieToCreate = Specie.Create(specieId, nameResult.Value);

        await specieRepository.Add(specieToCreate.Value, cancellationToken);

        logger.LogInformation("Create specie with id: {SpecieId}", specieToCreate.Value.Id);

        return (Guid)specieToCreate.Value.Id;
    }
}
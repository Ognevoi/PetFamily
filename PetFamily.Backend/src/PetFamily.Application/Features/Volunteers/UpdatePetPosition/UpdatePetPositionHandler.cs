using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extentions;
using PetFamily.Application.Features.Volunteers.Update;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.UpdatePetPosition;

public class UpdatePetPositionHandler : ICommandHandler<Guid, UpdatePetPositionCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<UpdatePetPositionCommand> _validator;
    private readonly ILogger<UpdateVolunteerHandler> _logger;

    public UpdatePetPositionHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<UpdatePetPositionCommand> validator,
        ILogger<UpdateVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        UpdatePetPositionCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var pet = volunteerResult.Value.Pets.FirstOrDefault(p => p.Id == command.PetId);
        if (pet == null)
            return Errors.General.NotFound(command.PetId).ToErrorList();

        var newPositionResult = Position.Create(command.NewPosition);
        if (newPositionResult.IsFailure)
            return newPositionResult.Error.ToErrorList();

        volunteerResult.Value.MovePet(pet, newPositionResult.Value);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var result = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation(
            "Pet with id: {PetId} was moved to position: {Position} from volunteer with id: {VolunteerId}",
            command.PetId, command.NewPosition, volunteerResult.Value.Id);

        return result;
    }
}
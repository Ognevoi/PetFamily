using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Features.Volunteers.Commands.Update;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Commands.UpdatePetPosition;

public class UpdatePetPositionHandler : ICommandHandler<UpdatePetPositionCommand, Guid>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<UpdateVolunteerHandler> _logger;

    public UpdatePetPositionHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<UpdateVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdatePetPositionCommand command,
        CancellationToken cancellationToken = default)
    {
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
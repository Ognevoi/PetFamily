using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Volunteers.Commands.UpdatePetPosition;

public record UpdatePetPositionCommand(
    Guid VolunteerId,
    Guid PetId,
    int NewPosition
    ) : ICommand;
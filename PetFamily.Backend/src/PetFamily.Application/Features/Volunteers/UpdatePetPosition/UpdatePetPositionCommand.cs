using PetFamily.Application.Features.Volunteers.Update;
using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Volunteers.UpdatePetPosition;

public record UpdatePetPositionCommand(
    Guid VolunteerId,
    Guid PetId,
    int NewPosition
    ) : ICommand;
using PetFamily.Application.Features.Volunteers.Update;

namespace PetFamily.Application.Features.Volunteers.UpdatePetPosition;

public record UpdatePetPositionRequest(
    Guid VolunteerId,
    Guid PetId,
    UpdatePetPositionDto Dto
    );

public record UpdatePetPositionDto(
    int NewPosition
);
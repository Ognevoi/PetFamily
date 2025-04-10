namespace PetFamily.Application.Features.Volunteers.GetPetPhoto;

public record GetPetPhotosRequest(
    Guid VolunteerId,
    Guid PetId);
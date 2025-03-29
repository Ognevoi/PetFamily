namespace PetFamily.Application.Volunteers.GetPetPhoto;

public record GetPetPhotosRequest(
    Guid VolunteerId,
    Guid PetId);
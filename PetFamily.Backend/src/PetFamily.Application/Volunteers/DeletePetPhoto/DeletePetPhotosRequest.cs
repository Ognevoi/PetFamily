namespace PetFamily.Application.Volunteers.DeletePetPhoto;

public record DeletePetPhotosRequest(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<string> PhotoNames);
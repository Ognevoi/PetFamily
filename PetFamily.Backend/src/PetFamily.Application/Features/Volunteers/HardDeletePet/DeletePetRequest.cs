namespace PetFamily.Application.Features.Volunteers.HardDeletePet;

public record DeletePetRequest(Guid VolunteerId, Guid PetId);
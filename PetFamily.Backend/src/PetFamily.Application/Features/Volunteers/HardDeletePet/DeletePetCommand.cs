using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Volunteers.HardDeletePet;

public record DeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;
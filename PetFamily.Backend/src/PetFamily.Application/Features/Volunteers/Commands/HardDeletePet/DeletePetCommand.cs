using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Volunteers.Commands.HardDeletePet
{
    public record DeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand<Guid>;
}
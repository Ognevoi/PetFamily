using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Volunteers.Commands.SoftDelete
{
    public record SoftDeleteVolunteerCommand(Guid VolunteerId) : ICommand<Guid>;
}
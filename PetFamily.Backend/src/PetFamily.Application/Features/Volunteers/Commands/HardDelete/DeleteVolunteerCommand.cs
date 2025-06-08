using PetFamily.Application.Interfaces;


namespace PetFamily.Application.Features.Volunteers.Commands.HardDelete
{
    public record DeleteVolunteerCommand(Guid VolunteerId) : ICommand;
}
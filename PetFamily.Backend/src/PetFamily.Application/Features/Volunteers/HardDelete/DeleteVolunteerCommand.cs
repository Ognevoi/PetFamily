using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Volunteers.HardDelete;

public record DeleteVolunteerCommand(Guid VolunteerId) : ICommand;
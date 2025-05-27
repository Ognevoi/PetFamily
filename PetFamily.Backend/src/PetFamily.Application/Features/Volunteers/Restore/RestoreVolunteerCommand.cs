using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Volunteers.Restore;

public record RestoreVolunteerCommand(Guid VolunteerId) : ICommand;
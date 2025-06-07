using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Volunteers.Commands.Restore;

public record RestoreVolunteerCommand(Guid VolunteerId) : ICommand;
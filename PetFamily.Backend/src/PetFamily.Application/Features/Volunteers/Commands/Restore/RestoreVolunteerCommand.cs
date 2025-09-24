using CSharpFunctionalExtensions;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Commands.Restore;

public record RestoreVolunteerCommand(Guid VolunteerId) : ICommand<Guid>;
using PetFamily.Application.Features.Volunteers.Commands.DTO;
using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Volunteers.Commands.Update;

public record UpdateVolunteerCommand(
    Guid VolunteerId,
    FullNameDto FullName,
    string Email,
    string Description,
    int ExperienceYears,
    string PhoneNumber)
    : ICommand<Guid>;
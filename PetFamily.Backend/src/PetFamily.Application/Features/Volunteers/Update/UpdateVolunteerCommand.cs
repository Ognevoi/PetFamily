using PetFamily.Application.Features.Volunteers.DTO;
using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Volunteers.Update;

public record UpdateVolunteerCommand(
    Guid VolunteerId,
    FullNameDto FullName,
    string Email,
    string Description,
    int ExperienceYears,
    string PhoneNumber)
    : ICommand;
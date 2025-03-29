using PetFamily.Application.Volunteers.DTO;

namespace PetFamily.Application.Volunteers.Update;

public record UpdateVolunteerRequest(
    Guid VolunteerId,
    UpdateVolunteerDto Dto
    );

public record UpdateVolunteerDto(
    FullNameDto FullName,
    string Email,
    string Description,
    int ExperienceYears,
    string PhoneNumber
);
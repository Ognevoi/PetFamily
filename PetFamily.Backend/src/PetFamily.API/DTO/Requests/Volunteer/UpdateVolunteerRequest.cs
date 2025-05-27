using PetFamily.Application.Features.Volunteers.DTO;
using PetFamily.Application.Features.Volunteers.Update;

namespace PetFamily.API.DTO.Requests.Volunteer;

public sealed record UpdateVolunteerRequest(
    FullNameDto FullName,
    string Email,
    string Description,
    int ExperienceYears,
    string PhoneNumber);

public static class UpdateVolunteerRequestExtensions
{
    public static UpdateVolunteerCommand ToCommand(this UpdateVolunteerRequest request, Guid id)
        => new UpdateVolunteerCommand(
            id,
            new FullNameDto(request.FullName.FirstName, request.FullName.LastName),
            request.Email,
            request.Description,
            request.ExperienceYears,
            request.PhoneNumber);
}
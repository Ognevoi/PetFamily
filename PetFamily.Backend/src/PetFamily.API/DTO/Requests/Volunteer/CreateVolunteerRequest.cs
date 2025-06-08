using PetFamily.Application.Features.Volunteers.Commands.Create;
using PetFamily.Application.Features.Volunteers.Commands.DTO;
using PetFamily.Domain.PetManagement.ValueObjects;

namespace PetFamily.API.DTO.Requests.Volunteer;

public sealed record CreateVolunteerRequest(
    string FirstName,
    string LastName,
    string Email,
    string Description,
    int ExperienceYears,
    string PhoneNumber,
    IEnumerable<SocialNetworkDto> SocialNetworks,
    IEnumerable<AssistanceDetailsDto> AssistanceDetails);

public static class CreateVolunteerRequestExtensions
{
    public static CreateVolunteerCommand ToCommand(this CreateVolunteerRequest request)
        => new CreateVolunteerCommand(
            VolunteerId.NewVolunteerId(),
            new FullNameDto(request.FirstName, request.LastName),
            request.Email,
            request.Description,
            request.ExperienceYears,
            request.PhoneNumber,
            request.SocialNetworks,
            request.AssistanceDetails);
}
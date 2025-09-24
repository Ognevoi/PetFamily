using PetFamily.Application.Features.Volunteers.Commands.DTO;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.PetManagement.ValueObjects;

namespace PetFamily.Application.Features.Volunteers.Commands.Create;

public sealed record CreateVolunteerCommand(
    VolunteerId VolunteerId,
    FullNameDto FullName,
    string Email,
    string Description,
    int ExperienceYears,
    string PhoneNumber,
    IEnumerable<SocialNetworkDto> SocialNetworks,
    IEnumerable<AssistanceDetailsDto> AssistanceDetails)
    : ICommand<Guid>;
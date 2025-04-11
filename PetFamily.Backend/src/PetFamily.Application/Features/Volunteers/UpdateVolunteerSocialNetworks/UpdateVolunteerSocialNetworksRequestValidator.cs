using FluentValidation;
using PetFamily.Application.Features.Volunteers.DTO;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.UpdateVolunteerSocialNetworks;

public class UpdateVolunteerSocialNetworksRequestValidator : AbstractValidator<UpdateVolunteerSocialNetworksRequest>
{
    public UpdateVolunteerSocialNetworksRequestValidator()
    {
        RuleFor(x => x.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        // TODO: Fix "Serialized error is invalid" when SocialNetworks values are empty
        RuleForEach(x => x.Dto.SocialNetworks).SetValidator(new SocialNetworkDtoValidator());
    }
}
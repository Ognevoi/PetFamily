using FluentValidation;
using PetFamily.Application.Features.Volunteers.Commands.DTO;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Commands.UpdateVolunteerSocialNetworks;

public class UpdateVolunteerSocialNetworksCommandValidator : AbstractValidator<UpdateVolunteerSocialNetworksCommand>
{
    public UpdateVolunteerSocialNetworksCommandValidator()
    {
        RuleFor(x => x.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        // TODO: Fix "Serialized error is invalid" when SocialNetworks values are empty
        RuleForEach(x => x.SocialNetworks).SetValidator(new SocialNetworkDtoValidator());
    }
}
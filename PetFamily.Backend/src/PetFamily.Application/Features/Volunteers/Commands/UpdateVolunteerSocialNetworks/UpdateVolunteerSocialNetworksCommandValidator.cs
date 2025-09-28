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
        
        RuleForEach(x => x.SocialNetworks)
            .SetValidator(new SocialNetworkDtoValidator())
            .When(x => x.SocialNetworks != null && x.SocialNetworks.Any());
    }
}
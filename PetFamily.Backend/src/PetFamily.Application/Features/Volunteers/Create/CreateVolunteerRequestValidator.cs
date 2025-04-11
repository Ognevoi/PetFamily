using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.PetManagement.ValueObjects;

namespace PetFamily.Application.Features.Volunteers.Create;

public class CreateVolunteerRequestValidator: AbstractValidator<CreateVolunteerRequest>
{
    public CreateVolunteerRequestValidator()
    {
        RuleFor(c => c.FullName).MustBeValueObject(f => FullName.Create(f.FirstName, f.LastName ));
        RuleFor(c => c.Email).MustBeValueObject(Email.Create);
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
        RuleFor(c => c.ExperienceYears).MustBeValueObject(ExperienceYears.Create);
        RuleFor(c => c.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
        RuleForEach(c => c.SocialNetworks).MustBeValueObject(s => SocialNetwork.Create(s.Name, s.Url));
        RuleForEach(c => c.AssistanceDetails).MustBeValueObject(a => AssistanceDetails.Create(a.Name, a.Description));
    }
}
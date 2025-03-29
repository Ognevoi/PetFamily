using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Update;

public class UpdateVolunteerRequestValidator: AbstractValidator<UpdateVolunteerRequest>
{
    public UpdateVolunteerRequestValidator()
    {
        RuleFor(x => x.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }   
}

public class UpdateVolunteerDtoValidator: AbstractValidator<UpdateVolunteerDto>
{
    public UpdateVolunteerDtoValidator()
    {
        RuleFor(c => c.FullName).MustBeValueObject(f => FullName.Create(f.FirstName, f.LastName ));
        RuleFor(c => c.Email).MustBeValueObject(Email.Create);
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
        RuleFor(c => c.ExperienceYears).MustBeValueObject(ExperienceYears.Create);
        RuleFor(c => c.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
    }   
}
using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Restore;

public class RestoreVolunteerRequestValidator : AbstractValidator<RestoreVolunteerRequest>
{
    public RestoreVolunteerRequestValidator()
    {
        RuleFor(x => x.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}
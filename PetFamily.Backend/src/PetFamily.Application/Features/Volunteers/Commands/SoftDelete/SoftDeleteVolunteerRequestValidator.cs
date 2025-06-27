using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Commands.SoftDelete;

public class SoftDeleteVolunteerCommandValidator : AbstractValidator<SoftDeleteVolunteerCommand>
{
    public SoftDeleteVolunteerCommandValidator()
    {
        RuleFor(x => x.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}
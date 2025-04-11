using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.HardDelete;

public class DeleteVolunteerRequestValidator : AbstractValidator<DeleteVolunteerRequest>
{
    public DeleteVolunteerRequestValidator()
    {
        RuleFor(x => x.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}
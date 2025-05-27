using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.UpdatePetPosition;

public class UpdatePetPositionCommandValidator: AbstractValidator<UpdatePetPositionCommand>
{
    public UpdatePetPositionCommandValidator()
    {
        RuleFor(x => x.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(x => x.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.NewPosition).MustBeValueObject(Position.Create);
    }   
}
using FluentValidation;
using PetFamily.Application.Features.Volunteers.Update;
using PetFamily.Application.Validation;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.UpdatePetPosition;

public class UpdatePetPositionRequestValidator: AbstractValidator<UpdatePetPositionRequest>
{
    public UpdatePetPositionRequestValidator()
    {
        RuleFor(x => x.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(x => x.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }   
}

public class UpdatePetPositionDtoValidator: AbstractValidator<UpdatePetPositionDto>
{
    public UpdatePetPositionDtoValidator()
    {
        RuleFor(c => c.NewPosition).MustBeValueObject(Position.Create);
    }   
}
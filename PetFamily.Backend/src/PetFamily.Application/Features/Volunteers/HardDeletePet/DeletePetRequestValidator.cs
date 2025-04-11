using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.HardDeletePet;

public class DeletePetRequestValidator : AbstractValidator<DeletePetRequest>
{
    public DeletePetRequestValidator()
    {
        RuleFor(x => x.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(x => x.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}
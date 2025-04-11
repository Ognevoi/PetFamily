using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.DeletePetPhoto;

public class DeletePetPhotosRequestValidator : AbstractValidator<DeletePetPhotosRequest>
{
    public DeletePetPhotosRequestValidator()
    {
        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.PhotoNames).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}
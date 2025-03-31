using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UploadPetPhoto;

public class UploadPetPhotosRequestValidator : AbstractValidator<UploadPetPhotosRequest>
{
    public UploadPetPhotosRequestValidator()
    {
        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.Photos).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}
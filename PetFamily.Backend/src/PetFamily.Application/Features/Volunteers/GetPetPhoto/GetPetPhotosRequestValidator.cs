using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.GetPetPhoto;

public class GetPetPhotosRequestValidator : AbstractValidator<GetPetPhotosRequest>
{
    public GetPetPhotosRequestValidator()
    {
        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());;
    }
}
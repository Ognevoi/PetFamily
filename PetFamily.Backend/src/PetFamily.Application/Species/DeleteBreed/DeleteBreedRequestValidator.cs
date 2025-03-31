using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.DeleteBreed;

public class DeleteBreedRequestValidator : AbstractValidator<DeleteBreedRequest>
{
    public DeleteBreedRequestValidator()
    {
        RuleFor(x => x.SpecieId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(x => x.BreedId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}
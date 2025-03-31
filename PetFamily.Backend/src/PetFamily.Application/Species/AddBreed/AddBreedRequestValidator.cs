using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.AddBreed;

public class AddBreedRequestValidator : AbstractValidator<AddBreedRequest>
{
    public AddBreedRequestValidator()
    {
        RuleFor(x => x.SpecieId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(x => x.Dto).NotNull().SetValidator(new AddBreedDtoValidator());
    }
}
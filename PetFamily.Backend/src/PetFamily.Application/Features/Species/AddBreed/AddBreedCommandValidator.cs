using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Species.AddBreed;

public class AddBreedCommandValidator : AbstractValidator<AddBreedCommand>
{
    public AddBreedCommandValidator()
    {
        RuleFor(x => x.SpecieId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(x => x.Name).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}
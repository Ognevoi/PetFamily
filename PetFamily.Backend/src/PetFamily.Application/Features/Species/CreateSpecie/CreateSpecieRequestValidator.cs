using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Features.Species.CreateSpecie;

public class CreateSpecieRequestValidator : AbstractValidator<CreateSpecieRequest>
{
    public CreateSpecieRequestValidator()
    {
        RuleFor(s => s.Name).MustBeValueObject(Name.Create);
    }
}
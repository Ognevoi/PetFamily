using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Features.Species.CreateSpecie;

public class CreateSpecieCommandValidator : AbstractValidator<CreateSpecieCommand>
{
    public CreateSpecieCommandValidator()
    {
        RuleFor(s => s.Name).MustBeValueObject(Name.Create);
    }
}
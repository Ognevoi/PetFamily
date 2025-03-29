using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.DeleteSpecie;

public class DeleteSpecieRequestValidator : AbstractValidator<DeleteSpecieRequest>
{
    public DeleteSpecieRequestValidator()
    {
        RuleFor(s => s.SpecieId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}
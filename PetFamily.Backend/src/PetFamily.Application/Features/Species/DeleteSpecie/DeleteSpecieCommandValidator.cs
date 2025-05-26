using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Species.DeleteSpecie;

public class DeleteSpecieCommandValidator : AbstractValidator<DeleteSpecieCommand>
{
    public DeleteSpecieCommandValidator()
    {
        RuleFor(s => s.SpecieId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}
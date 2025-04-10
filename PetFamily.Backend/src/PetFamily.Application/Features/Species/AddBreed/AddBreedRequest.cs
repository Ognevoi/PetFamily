using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Species.AddBreed;

public record AddBreedRequest(
    Guid SpecieId,
    AddBreedDto Dto
);

public record AddBreedDto(
    string Name
);

public class AddBreedDtoValidator : AbstractValidator<AddBreedDto>
{
    public AddBreedDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}
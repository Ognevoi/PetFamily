using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.DTO;

public record FullNameDto
{
    public string FirstName { get; }
    public string LastName { get; }

    public FullNameDto(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}

public class FullNameDtoValidator : AbstractValidator<FullNameDto>
{
    public FullNameDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(x => x.LastName).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}
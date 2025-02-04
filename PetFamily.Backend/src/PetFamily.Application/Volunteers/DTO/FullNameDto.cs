using FluentValidation;

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
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
    }
}
using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Commands.DTO;

public record AssistanceDetailsDto
{
    public string Name { get; }
    public string Description { get; }

    public AssistanceDetailsDto(string name, string description)
    {
        Name = name;
        Description = description;
    }
}

public class AssistanceDetailsDtoValidator : AbstractValidator<AssistanceDetailsDto>
{
    public AssistanceDetailsDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(x => x.Description).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}
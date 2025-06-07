using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Commands.DTO;

public record SocialNetworkDto
{
    public string Name { get; }
    public string Url { get; }

    public SocialNetworkDto(string name, string url)
    {
        Name = name;
        Url = url;
    }
}

public class SocialNetworkDtoValidator : AbstractValidator<SocialNetworkDto>
{
    public SocialNetworkDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(x => x.Url).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}
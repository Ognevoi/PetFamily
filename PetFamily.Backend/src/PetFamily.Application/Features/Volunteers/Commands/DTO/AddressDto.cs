using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Commands.DTO;

public record AddressDto
{
    public string Street { get; }
    public string City { get; }
    public string State { get; }
    public string ZipCode { get; }
    
    public AddressDto(string street, string city, string state, string zipCode)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
    }
}

public class AddressDtoValidator: AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(x => x.Street).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(x => x.City).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(x => x.State).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(x => x.ZipCode).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}


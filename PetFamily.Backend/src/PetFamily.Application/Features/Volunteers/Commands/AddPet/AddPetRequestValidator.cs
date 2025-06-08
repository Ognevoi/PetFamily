using FluentValidation;
using PetFamily.Application.Features.Volunteers.Commands.DTO;
using PetFamily.Application.Validation;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Commands.AddPet;


public class AddPetCommandValidator : AbstractValidator<AddPetCommand>
{
    public AddPetCommandValidator()
    {
        RuleFor(c => c.Name).MustBeValueObject(PetName.Create);
        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.SpecieId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.BreedId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
        RuleFor(c => c.PetColor).MustBeValueObject(PetColor.Create);
        RuleFor(c => c.PetHealth).MustBeValueObject(PetHealthInfo.Create);
        RuleFor(c => c.Weight).MustBeValueObject(Weight.Create);
        RuleFor(c => c.Height).MustBeValueObject(Height.Create);
        RuleFor(c => c.Address).SetValidator(new AddressDtoValidator());
        RuleFor(c => c.BirthDate).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.PetStatus).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.IsSterilized).MustBeValueObject(IsSterilized.Create);
        RuleFor(c => c.IsVaccinated).MustBeValueObject(IsVaccinated.Create);
    }
}

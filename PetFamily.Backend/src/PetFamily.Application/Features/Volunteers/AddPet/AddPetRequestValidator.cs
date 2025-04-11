using FluentValidation;
using PetFamily.Application.Features.Volunteers.DTO;
using PetFamily.Application.Validation;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.AddPet;


public class AddPetRequestValidator : AbstractValidator<AddPetRequest>
{
    public AddPetRequestValidator()
    {
        RuleFor(c => c.Dto.Name).MustBeValueObject(PetName.Create);
        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.Dto.SpecieId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.Dto.BreedId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.Dto.Description).MustBeValueObject(Description.Create);
        RuleFor(c => c.Dto.PetColor).MustBeValueObject(PetColor.Create);
        RuleFor(c => c.Dto.PetHealth).MustBeValueObject(PetHealthInfo.Create);
        RuleFor(c => c.Dto.Weight).MustBeValueObject(Weight.Create);
        RuleFor(c => c.Dto.Height).MustBeValueObject(Height.Create);
        RuleFor(c => c.Dto.Address).SetValidator(new AddressDtoValidator());
        RuleFor(c => c.Dto.BirthDate).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.Dto.PetStatus).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.Dto.IsSterilized).MustBeValueObject(IsSterilized.Create);
        RuleFor(c => c.Dto.IsVaccinated).MustBeValueObject(IsVaccinated.Create);
    }
}

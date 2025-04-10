using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.UpdateVolunteerAssistanceDetails;

public class UpdateVolunteerAssistanceDetailsRequestValidator : AbstractValidator<UpdateVolunteerAssistanceDetailsRequest>
{
    public UpdateVolunteerAssistanceDetailsRequestValidator()
    {
        RuleFor(x => x.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        // TODO: Fix "Serialized error is invalid" when AssistanceDetails values are empty
        // RuleForEach(x => x.Dto.AssistanceDetails).SetValidator(new AssistanceDetailDtoValidator());
    }
}
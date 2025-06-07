using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Commands.UpdateVolunteerAssistanceDetails;

public class UpdateVolunteerAssistanceDetailsCommandValidator : AbstractValidator<UpdateVolunteerAssistanceDetailsCommand>
{
    public UpdateVolunteerAssistanceDetailsCommandValidator()
    {
        RuleFor(x => x.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        // TODO: Fix "Serialized error is invalid" when AssistanceDetails values are empty
        // RuleForEach(x => x.Dto.AssistanceDetails).SetValidator(new AssistanceDetailDtoValidator());
    }
}
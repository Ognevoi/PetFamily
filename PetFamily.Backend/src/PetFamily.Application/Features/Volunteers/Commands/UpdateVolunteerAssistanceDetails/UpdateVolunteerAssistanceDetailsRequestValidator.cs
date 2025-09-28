using FluentValidation;
using PetFamily.Application.Features.Volunteers.Commands.DTO;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Commands.UpdateVolunteerAssistanceDetails;

public class
    UpdateVolunteerAssistanceDetailsCommandValidator : AbstractValidator<UpdateVolunteerAssistanceDetailsCommand>
{
    public UpdateVolunteerAssistanceDetailsCommandValidator()
    {
        RuleFor(x => x.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleForEach(x => x.AssistanceDetails)
            .SetValidator(new AssistanceDetailsDtoValidator())
            .When(x => x.AssistanceDetails != null && x.AssistanceDetails.Any());
    }
}
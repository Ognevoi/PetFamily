using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Features.Volunteers.Update;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.UpdateVolunteerAssistanceDetails;

public class UpdateVolunteerAssistanceDetailsHandler(
    IVolunteersRepository volunteersRepository,
    IValidator<UpdateVolunteerAssistanceDetailsCommand> validator,
    ILogger<UpdateVolunteerHandler> logger)
    : ICommandHandler<Guid, UpdateVolunteerAssistanceDetailsCommand>
{
    public async Task<Result<Guid, ErrorList>> HandleAsync(
        UpdateVolunteerAssistanceDetailsCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var assistanceDetailsResult =
            command.AssistanceDetails.Select(sn => AssistanceDetails.Create(sn.Name, sn.Description).Value);

        volunteerResult.Value.UpdateAssistanceDetails(assistanceDetailsResult);

        var result = await volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        logger.LogInformation(
            "Update volunteer assistance details: " +
            "volunteer id: {VolunteerId}, " +
            "assistance details: {AssistanceDetails}",
            volunteerResult.Value.Id,
            string.Join(", ", command.AssistanceDetails.Select(sn => sn.Name)));

        return result;
    }
}
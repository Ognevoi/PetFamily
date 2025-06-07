using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Features.Volunteers.Commands.Update;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Commands.UpdateVolunteerAssistanceDetails;

public class UpdateVolunteerAssistanceDetailsHandler : ICommandHandler<Guid, UpdateVolunteerAssistanceDetailsCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<UpdateVolunteerAssistanceDetailsCommand> _validator;
    private readonly ILogger<UpdateVolunteerHandler> _logger;

    public UpdateVolunteerAssistanceDetailsHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<UpdateVolunteerAssistanceDetailsCommand> validator,
        ILogger<UpdateVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        UpdateVolunteerAssistanceDetailsCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var assistanceDetailsResult =
            command.AssistanceDetails.Select(sn => AssistanceDetails.Create(sn.Name, sn.Description).Value);

        volunteerResult.Value.UpdateAssistanceDetails(assistanceDetailsResult);

        var result = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation(
            "Update volunteer assistance details: " +
            "volunteer id: {VolunteerId}, " +
            "assistance details: {AssistanceDetails}",
            volunteerResult.Value.Id,
            string.Join(", ", command.AssistanceDetails.Select(sn => sn.Name)));

        return result;
    }
}
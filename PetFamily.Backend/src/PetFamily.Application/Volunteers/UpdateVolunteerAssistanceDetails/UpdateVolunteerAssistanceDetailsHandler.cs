using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Volunteers.Update;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UpdateVolunteerAssistanceDetails;

public class UpdateVolunteerAssistanceDetailsHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<UpdateVolunteerHandler> _logger;

    public UpdateVolunteerAssistanceDetailsHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<UpdateVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateVolunteerAssistanceDetailsRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(request.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var assistanceDetailsResult =
            request.Dto.AssistanceDetails.Select(sn => AssistanceDetails.Create(sn.Name, sn.Description).Value);

        volunteerResult.Value.UpdateAssistanceDetails(assistanceDetailsResult);

        var result = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        // _logger.LogInformation(
        //     "Update volunteer assistance details: " +
        //     "volunteer id: {VolunteerId}, " +
        //     "assistance details: {AssistanceDetails}",
        //     volunteerResult.Value.Id,
        //     string.Join(", ", volunteerResult.Value.AssistanceDetails.Select(sn => sn.Name)));

        return result;
    }
}
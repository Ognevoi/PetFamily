using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extentions;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.HardDelete;

public class HardDeleteVolunteerHandler(
    IVolunteersRepository volunteersRepository,
    DeleteVolunteerCommandValidator validator,
    ILogger<HardDeleteVolunteerHandler> logger)
    : ICommandHandler<Guid, DeleteVolunteerCommand>
{
    public async Task<Result<Guid, ErrorList>> HandleAsync(
        DeleteVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var volunteerResult = await volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        await volunteersRepository.Delete(volunteerResult.Value, cancellationToken);

        await volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        logger.LogInformation("Volunteer with id: {VolunteerId} was HARD deleted", volunteerResult.Value.Id);

        return volunteerResult.Value.Id.Value;
    }

}
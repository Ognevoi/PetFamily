using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Restore;

public class RestoreVolunteerHandler(
    IVolunteersRepository volunteersRepository,
    IValidator<RestoreVolunteerCommand> validator,
    ILogger<RestoreVolunteerHandler> logger)
    : ICommandHandler<Guid, RestoreVolunteerCommand>
{
    public async Task<Result<Guid, ErrorList>> HandleAsync(
        RestoreVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        volunteerResult.Value.Restore();

        await volunteersRepository.Save(volunteerResult.Value, cancellationToken);
        
        logger.LogInformation("Restore volunteer with id: {VolunteerId}", volunteerResult.Value.Id);

        return volunteerResult.Value.Id.Value;
    }
}
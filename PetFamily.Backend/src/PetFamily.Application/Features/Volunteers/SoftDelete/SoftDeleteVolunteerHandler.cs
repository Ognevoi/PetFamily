using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extentions;
using PetFamily.Application.Features.Volunteers.HardDelete;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.SoftDelete;

public class SoftDeleteVolunteerHandler(
    IVolunteersRepository volunteersRepository,
    DeleteVolunteerCommandValidator validator,
    ILogger<SoftDeleteVolunteerHandler> logger)
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
        
        volunteerResult.Value.SoftDelete();
        
        await volunteersRepository.Save(volunteerResult.Value, cancellationToken);
        
        logger.LogInformation("Volunteer with id: {VolunteerId} was SOFT deleted", volunteerResult.Value.Id);

        return volunteerResult.Value.Id.Value;
    }
    
}
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Features.Volunteers.HardDelete;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.SoftDelete;

public class SoftDeleteVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<SoftDeleteVolunteerHandler> _logger;

    public SoftDeleteVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<SoftDeleteVolunteerHandler> logger
        )
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }
    
    public async Task<Result<Guid, Error>> Handle(
        DeleteVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(request.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;
        
        volunteerResult.Value.SoftDelete();
        
        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation("Volunteer with id: {VolunteerId} was SOFT deleted", volunteerResult.Value.Id);

        return volunteerResult.Value.Id.Value;
    }
    
}
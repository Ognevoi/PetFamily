using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.HardDelete;

public class HardDeleteVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<HardDeleteVolunteerHandler> _logger;

    public HardDeleteVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<HardDeleteVolunteerHandler> logger
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
        
        await _volunteersRepository.Delete(volunteerResult.Value, cancellationToken);
        
        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation("Volunteer with id: {VolunteerId} was HARD deleted", volunteerResult.Value.Id);

        return volunteerResult.Value.Id.Value;
    }
    
}
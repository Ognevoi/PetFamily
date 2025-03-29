using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Restore;

public class RestoreVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<RestoreVolunteerHandler> _logger;

    public RestoreVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<RestoreVolunteerHandler> logger
        )
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }
    
    public async Task<Result<Guid, Error>> Handle(
        RestoreVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(request.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;
        
        volunteerResult.Value.Restore();

        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation("Respote volunteer with id: {VolunteerId}", volunteerResult.Value.Id);

        return volunteerResult.Value.Id.Value;
    }
}
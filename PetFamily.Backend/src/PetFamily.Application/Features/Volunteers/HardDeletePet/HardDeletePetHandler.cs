using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.HardDeletePet;

public class HardDeletePetHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<HardDeletePetHandler> _logger;

    public HardDeletePetHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<HardDeletePetHandler> logger
        )
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }
    
    public async Task<Result<Guid, Error>> Handle(
        DeletePetRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(request.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;
        
        var pet = volunteerResult.Value.Pets.FirstOrDefault(p => p.Id == request.PetId);
        if (pet == null)
            return Errors.General.NotFound(request.PetId);

        volunteerResult.Value.RemovePet(pet);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;
        
        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation(
            "Pet with id: {PetId} was HARD deleted from volunteer with id: {VolunteerId}",
            request.PetId, volunteerResult.Value.Id);

        return volunteerResult.Value.Id.Value;
    }
    
}
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Features.Volunteers.Update;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.UpdatePetPosition;

public class UpdatePetPositionHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<UpdateVolunteerHandler> _logger;

    public UpdatePetPositionHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<UpdateVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }
    
    public async Task<Result<Guid, Error>> Handle(
        UpdatePetPositionRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(request.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;
        
        var pet = volunteerResult.Value.Pets.FirstOrDefault(p => p.Id == request.PetId);
        if (pet == null)
            return Errors.General.NotFound(request.PetId);

        var newPositionResult = Position.Create(request.Dto.NewPosition);
        if (newPositionResult.IsFailure)
            return newPositionResult.Error;

        volunteerResult.Value.MovePet(pet, newPositionResult.Value);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var result = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation(
            "Pet with id: {PetId} was moved to position: {Position} from volunteer with id: {VolunteerId}",
            request.PetId, request.Dto.NewPosition, volunteerResult.Value.Id);

        return result;
    }
}
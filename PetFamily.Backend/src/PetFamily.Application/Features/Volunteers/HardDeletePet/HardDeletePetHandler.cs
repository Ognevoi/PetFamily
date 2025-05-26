using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.HardDeletePet;

public class HardDeletePetHandler(
    IVolunteersRepository volunteersRepository,
    IValidator<DeletePetCommand> validator,
    ILogger<HardDeletePetHandler> logger)
    : ICommandHandler<Guid, DeletePetCommand>
{
    public async Task<Result<Guid, ErrorList>> HandleAsync(
        DeletePetCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var pet = volunteerResult.Value.Pets.FirstOrDefault(p => p.Id == command.PetId);
        if (pet == null)
            return Errors.General.NotFound(command.PetId).ToErrorList();

        volunteerResult.Value.RemovePet(pet);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        await volunteersRepository.Save(volunteerResult.Value, cancellationToken);
        
        logger.LogInformation(
            "Pet with id: {PetId} was HARD deleted from volunteer with id: {VolunteerId}",
            command.PetId, volunteerResult.Value.Id);

        return volunteerResult.Value.Id.Value;
    }
    
}
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.HardDeletePet;

public class HardDeletePetHandler : ICommandHandler<Guid, DeletePetCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<DeletePetCommand> _validator;
    private readonly ILogger<HardDeletePetHandler> _logger;

    public HardDeletePetHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<DeletePetCommand> validator,
        ILogger<HardDeletePetHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        DeletePetCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var pet = volunteerResult.Value.Pets.FirstOrDefault(p => p.Id == command.PetId);
        if (pet == null)
            return Errors.General.NotFound(command.PetId).ToErrorList();

        volunteerResult.Value.RemovePet(pet);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation(
            "Pet with id: {PetId} was HARD deleted from volunteer with id: {VolunteerId}",
            command.PetId, volunteerResult.Value.Id);

        return volunteerResult.Value.Id.Value;
    }
    
}
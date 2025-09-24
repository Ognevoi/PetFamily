using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Commands.HardDeletePet;

public class HardDeletePetHandler : ICommandHandler<DeletePetCommand, Guid>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<HardDeletePetHandler> _logger;

    public HardDeletePetHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<HardDeletePetHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
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
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Restore;

public class RestoreVolunteerHandler : ICommandHandler<Guid, RestoreVolunteerCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<RestoreVolunteerCommand> _validator;
    private readonly ILogger<RestoreVolunteerHandler> _logger;

    public RestoreVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<RestoreVolunteerCommand> validator,
        ILogger<RestoreVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        RestoreVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        volunteerResult.Value.Restore();

        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Restore volunteer with id: {VolunteerId}", volunteerResult.Value.Id);

        return volunteerResult.Value.Id.Value;
    }
}
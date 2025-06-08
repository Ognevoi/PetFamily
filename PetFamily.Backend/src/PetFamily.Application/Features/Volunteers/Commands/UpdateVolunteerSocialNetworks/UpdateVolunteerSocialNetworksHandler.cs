using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.Features.Volunteers.Commands.Update;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Commands.UpdateVolunteerSocialNetworks;

public class UpdateVolunteerSocialNetworksHandler : ICommandHandler<Guid, UpdateVolunteerSocialNetworksCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<UpdateVolunteerSocialNetworksCommand> _validator;
    private readonly ILogger<UpdateVolunteerHandler> _logger;

    public UpdateVolunteerSocialNetworksHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<UpdateVolunteerSocialNetworksCommand> validator,
        ILogger<UpdateVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        UpdateVolunteerSocialNetworksCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var socialNetworksResult = command.SocialNetworks.Select(sn => SocialNetwork.Create(sn.Name, sn.Url).Value);

        volunteerResult.Value.UpdateSocialNetworks(socialNetworksResult);

        var result = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation(
            "Update volunteer social networks: " +
            "volunteer id: {VolunteerId}, " +
            "social networks: {SocialNetworks}",
            volunteerResult.Value.Id,
            string.Join(", ", command.SocialNetworks.Select(sn => $"{sn.Name}: {sn.Url}").ToArray()));

        return result;
    }
}
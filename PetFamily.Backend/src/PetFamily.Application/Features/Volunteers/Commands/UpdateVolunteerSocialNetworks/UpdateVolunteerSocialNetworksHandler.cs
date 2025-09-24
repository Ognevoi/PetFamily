using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Features.Volunteers.Commands.Update;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Commands.UpdateVolunteerSocialNetworks;

public class UpdateVolunteerSocialNetworksHandler : ICommandHandler<UpdateVolunteerSocialNetworksCommand, Guid>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<UpdateVolunteerHandler> _logger;

    public UpdateVolunteerSocialNetworksHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<UpdateVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateVolunteerSocialNetworksCommand command,
        CancellationToken cancellationToken = default)
    {
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
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extentions;
using PetFamily.Application.Features.Volunteers.Update;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.UpdateVolunteerSocialNetworks;

public class UpdateVolunteerSocialNetworksHandler(
    IVolunteersRepository volunteersRepository,
    IValidator<UpdateVolunteerSocialNetworksCommand> validator,
    ILogger<UpdateVolunteerHandler> logger)
    : ICommandHandler<Guid, UpdateVolunteerSocialNetworksCommand>
{
    public async Task<Result<Guid, ErrorList>> HandleAsync(
        UpdateVolunteerSocialNetworksCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerResult = await volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var socialNetworksResult = command.SocialNetworks.Select(sn => SocialNetwork.Create(sn.Name, sn.Url).Value);

        volunteerResult.Value.UpdateSocialNetworks(socialNetworksResult);

        var result = await volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        logger.LogInformation(
            "Update volunteer social networks: " +
            "volunteer id: {VolunteerId}, " +
            "social networks: {SocialNetworks}",
            volunteerResult.Value.Id,
            string.Join(", ", command.SocialNetworks.Select(sn => $"{sn.Name}: {sn.Url}").ToArray()));

        return result;
    }
}
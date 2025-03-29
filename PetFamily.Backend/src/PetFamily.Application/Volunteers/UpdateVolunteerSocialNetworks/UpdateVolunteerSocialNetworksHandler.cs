using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Volunteers.Update;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UpdateVolunteerSocialNetworks;

public class UpdateVolunteerSocialNetworksHandler
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
    
    public async Task<Result<Guid, Error>> Handle(
        UpdateVolunteerSocialNetworksRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(request.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var socialNetworksResult = request.Dto.SocialNetworks.Select(sn => SocialNetwork.Create(sn.Name, sn.Url).Value);
        
        volunteerResult.Value.UpdateSocialNetworks(socialNetworksResult);
        
        var result = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation(
            "Update volunteer social networks: " +
            "volunteer id: {VolunteerId}, " +
            "social networks: {SocialNetworks}",
            volunteerResult.Value.Id,
            string.Join(", ", request.Dto.SocialNetworks.Select(sn => $"{sn.Name}: {sn.Url}").ToArray()));
        
        return result;
    }
}
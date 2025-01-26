using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Application.Volunteers.DTO;


namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteerController : ControllerBase
{
    
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateVolunteerHandler handler,
        CreateVolunteerRequest request,
        CancellationToken cancellationToken)
    {

        var command = new CreateVolunteerCommand(
            FullName: request.FullName,
            Email: request.Email,
            Description: request.Description,
            ExperienceYears: request.ExperienceYears,
            PhoneNumber: request.PhoneNumber,
            SocialNetworks: request.SocialNetworks.Select(sn => new SocialNetworkDto(sn.Name, sn.Url)),
            AssistanceDetails: request.AssistanceDetails.Select(ad => new AssistanceDetailsDto(ad.Name, ad.Description))
            );
        
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}
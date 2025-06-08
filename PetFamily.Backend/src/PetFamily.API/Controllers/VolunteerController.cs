using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.DTO.Requests.Volunteer;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.Application.Features.Volunteers;
using PetFamily.Application.Features.Volunteers.Commands.AddPet;
using PetFamily.Application.Features.Volunteers.Commands.Create;
using PetFamily.Application.Features.Volunteers.Commands.DeletePetPhoto;
using PetFamily.Application.Features.Volunteers.Commands.GetPetPhoto;
using PetFamily.Application.Features.Volunteers.Commands.HardDelete;
using PetFamily.Application.Features.Volunteers.Commands.HardDeletePet;
using PetFamily.Application.Features.Volunteers.Commands.Restore;
using PetFamily.Application.Features.Volunteers.Commands.SoftDelete;
using PetFamily.Application.Features.Volunteers.Commands.Update;
using PetFamily.Application.Features.Volunteers.Commands.UpdatePetPosition;
using PetFamily.Application.Features.Volunteers.Commands.UpdateVolunteerAssistanceDetails;
using PetFamily.Application.Features.Volunteers.Commands.UpdateVolunteerSocialNetworks;
using PetFamily.Application.Features.Volunteers.Commands.UploadPetPhoto;
using PetFamily.Application.Features.Volunteers.Queries.GetPetById;
using PetFamily.Application.Features.Volunteers.Queries.GetVolunteerById;
using PetFamily.Application.Features.Volunteers.Queries.GetVolunteers;
using PetFamily.Domain.Shared;
using GetPetByIdHandler = PetFamily.Application.Features.Volunteers.Queries.GetPetById.GetPetByIdHandler;
using GetPetsHandler = PetFamily.Application.Features.Volunteers.Queries.GetPets.GetPetsHandler;


namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteerController : ApplicationController
{
    [HttpGet]
    public async Task<ActionResult> GetAll(
        [FromQuery] GetVolunteerWithPaginationRequest request,
        [FromServices] GetVolunteersHandler handler, 
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        var response = await handler.HandleAsync(query, cancellationToken);
        
        return response.IsFailure ? BadRequest(response.Error) : Ok(response.Value);  
    }
    
    [HttpGet("{volunteerId:guid}")]
    public async Task<ActionResult> GetById(
        [FromRoute] Guid volunteerId,
        [FromServices] GetVolunteerByIdHandler handler, 
        CancellationToken cancellationToken)
    {
        var query = new GetVolunteerByIdQuery(volunteerId);
        var response = await handler.HandleAsync(query, cancellationToken);
        
        return response.IsFailure ? BadRequest(response.Error) : Ok(response.Value);  
    }
    
    [HttpGet("pets")]
    public async Task<ActionResult> GetAllPets(
        [FromQuery] GetPetWithPaginationRequest request,
        [FromServices] GetPetsHandler handler, 
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        var response = await handler.HandleAsync(query, cancellationToken);
        
        return response.IsFailure ? BadRequest(response.Error) : Ok(response.Value);  
    }
    
    [HttpGet("pets/{petId:guid}")]
    public async Task<ActionResult> GetPetById(
        [FromRoute] Guid petId,
        [FromServices] GetPetByIdHandler handler, 
        CancellationToken cancellationToken)
    {
        var query = new GetPetByIdQuery(petId);
        var response = await handler.HandleAsync(query, cancellationToken);
        
        return response.IsFailure ? BadRequest(response.Error) : Ok(response.Value);  
    }
    
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromServices] CreateVolunteerHandler handler,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        
        var result = await handler.HandleAsync(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult> UpdateMainInfo(
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerRequest request,
        [FromServices] UpdateVolunteerHandler handler,
        CancellationToken cancellationToken)
    {
        
        var command = request.ToCommand(id);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpPut("{id:guid}/social-networks")]
    public async Task<ActionResult> UpdateSocialNetworks(
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerSocialNetworksRequest request,
        [FromServices] UpdateVolunteerSocialNetworksHandler handler,
        [FromServices] IValidator<UpdateVolunteerSocialNetworksRequest> validator,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpPut("{id:guid}/assistance-details")]
    public async Task<ActionResult> UpdateAssistanceDetails(
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerAssistanceDetailsRequest request,
        [FromServices] UpdateVolunteerAssistanceDetailsHandler handler,
        [FromServices] IValidator<UpdateVolunteerAssistanceDetailsRequest> validator,
        CancellationToken cancellationToken)
    {
        
        var command = request.ToCommand(id);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpDelete("{id:guid}/soft")]
    public async Task<ActionResult> SoftDelete(
        [FromRoute] Guid id,
        [FromServices] SoftDeleteVolunteerHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteVolunteerRequest().ToCommand(id);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpDelete("{id:guid}/hard")]
    public async Task<ActionResult> HardDelete(
        [FromRoute] Guid id,
        [FromServices] HardDeleteVolunteerHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteVolunteerRequest().ToCommand(id);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpPatch("{id:guid}/restore")]
    public async Task<ActionResult> Restore(
        [FromRoute] Guid id,
        [FromServices] RestoreVolunteerHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new RestoreVolunteerRequest().ToCommand(id);
        
        var result = await handler.HandleAsync(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpPost("{volunteerId:guid}/pet")]
    public async Task<ActionResult> AddPet(
        [FromRoute] Guid volunteerId,
        [FromBody] AddPetRequest request,
        [FromServices] AddPetHandler handler,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/hard")]
    public async Task<ActionResult> HardDeletePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] HardDeletePetHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeletePetRequest().ToCommand(volunteerId, petId);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpPatch("{volunteerId:guid}/pet/{petId:guid}/position")]
    public async Task<ActionResult> UpdatePetPosition(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] UpdatePetPositionRequest request,
        [FromServices] UpdatePetPositionHandler handler,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId, petId);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpGet("{volunteerId:guid}/pet/{petId:guid}/photo-links")]
    public async Task<ActionResult> GetPetPhotoLinks(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] GetPetPhotosHandler getPetPhotosHandler,
        [FromServices] IVolunteersRepository volunteersRepository,
        CancellationToken cancellationToken)
    {
        var volunteerResult = await volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToResponse();

        var petResult = volunteerResult.Value.GetPetById(petId);
        if (petResult.IsFailure)
            return petResult.Error.ToResponse();

        var command = new GetPetPhotosRequest().ToCommand(volunteerId, petId);

        var result = await getPetPhotosHandler.HandleAsync(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpPost("{volunteerId:guid}/pet/{petId:guid}/photos")]
    public async Task<ActionResult> UploadPetPhotos(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromForm] IFormFileCollection files,
        [FromServices] UploadPetPhotosHandler uploadPetPhotoHandler,
        CancellationToken cancellationToken)
    {
        if (files.Count == 0)
            return Errors.General.NoFileForProcessing().ToResponse();

        await using var fileProcessor = new FormFileProcessor();
        var fileDtos = fileProcessor.Process(files);

        var command = new UploadPetPhotosRequest().ToCommand(fileDtos, volunteerId, petId);

        var result = await uploadPetPhotoHandler.HandleAsync(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/photos")]
    public async Task<ActionResult> DeletePetPhotos(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] IEnumerable<string> photoNames,
        [FromServices] DeletePetPhotosHandler deletePetPhotosHandler,
        CancellationToken cancellationToken)
    {
        var command = new DeletePetPhotosRequest().ToCommand(volunteerId, petId, photoNames);

        var result = await deletePetPhotosHandler.HandleAsync(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
}
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.Application.Features.Volunteers;
using PetFamily.Application.Features.Volunteers.AddPet;
using PetFamily.Application.Features.Volunteers.Create;
using PetFamily.Application.Features.Volunteers.DeletePetPhoto;
using PetFamily.Application.Features.Volunteers.GetPetPhoto;
using PetFamily.Application.Features.Volunteers.HardDelete;
using PetFamily.Application.Features.Volunteers.HardDeletePet;
using PetFamily.Application.Features.Volunteers.Restore;
using PetFamily.Application.Features.Volunteers.SoftDelete;
using PetFamily.Application.Features.Volunteers.Update;
using PetFamily.Application.Features.Volunteers.UpdatePetPosition;
using PetFamily.Application.Features.Volunteers.UpdateVolunteerAssistanceDetails;
using PetFamily.Application.Features.Volunteers.UpdateVolunteerSocialNetworks;
using PetFamily.Application.Features.Volunteers.UploadPetPhoto;
using PetFamily.Domain.Shared;


namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteerController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromServices] CreateVolunteerHandler handler,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult> UpdateMainInfo(
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerDto dto,
        [FromServices] UpdateVolunteerHandler handler,
        [FromServices] IValidator<UpdateVolunteerRequest> validator,
        CancellationToken cancellationToken)
    {
        var request = new UpdateVolunteerRequest(id, dto);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.Handle(request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpPut("{id:guid}/social-networks")]
    public async Task<ActionResult> UpdateSocialNetworks(
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerSocialNetworksDto dto,
        [FromServices] UpdateVolunteerSocialNetworksHandler handler,
        [FromServices] IValidator<UpdateVolunteerSocialNetworksRequest> validator,
        CancellationToken cancellationToken)
    {
        var request = new UpdateVolunteerSocialNetworksRequest(id, dto);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.Handle(request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpPut("{id:guid}/assistance-details")]
    public async Task<ActionResult> UpdateAssistanceDetails(
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerAssistanceDetailsDto dto,
        [FromServices] UpdateVolunteerAssistanceDetailsHandler handler,
        [FromServices] IValidator<UpdateVolunteerAssistanceDetailsRequest> validator,
        CancellationToken cancellationToken)
    {
        var request = new UpdateVolunteerAssistanceDetailsRequest(id, dto);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.Handle(request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpDelete("{id:guid}/soft")]
    public async Task<ActionResult> SoftDelete(
        [FromRoute] Guid id,
        [FromServices] SoftDeleteVolunteerHandler handler,
        [FromServices] IValidator<DeleteVolunteerRequest> validator,
        CancellationToken cancellationToken)
    {
        var request = new DeleteVolunteerRequest(id);
        
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.Handle(request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpDelete("{id:guid}/hard")]
    public async Task<ActionResult> HardDelete(
        [FromRoute] Guid id,
        [FromServices] HardDeleteVolunteerHandler handler,
        [FromServices] IValidator<DeleteVolunteerRequest> validator,
        CancellationToken cancellationToken)
    {
        var request = new DeleteVolunteerRequest(id);
        
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.Handle(request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpPatch("{id:guid}/restore")]
    public async Task<ActionResult> Restore(
        [FromRoute] Guid id,
        [FromServices] RestoreVolunteerHandler handler,
        [FromServices] IValidator<RestoreVolunteerRequest> validator,
        CancellationToken cancellationToken)
    {
        var request = new RestoreVolunteerRequest(id);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.Handle(request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpPost("{volunteerId:guid}/pet")]
    public async Task<ActionResult> AddPet(
        [FromRoute] Guid volunteerId,
        [FromBody] AddPetDto requestDto,
        [FromServices] AddPetHandler handler,
        [FromServices] IValidator<AddPetRequest> validator,
        CancellationToken cancellationToken)
    {
        var request = new AddPetRequest(volunteerId, requestDto);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.Handle(request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/hard")]
    public async Task<ActionResult> HardDeletePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] HardDeletePetHandler handler,
        [FromServices] IValidator<DeletePetRequest> validator,
        CancellationToken cancellationToken)
    {
        var request = new DeletePetRequest(volunteerId, petId);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.Handle(request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpPatch("{volunteerId:guid}/pet/{petId:guid}/position")]
    public async Task<ActionResult> UpdatePetPosition(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] UpdatePetPositionDto dto,
        [FromServices] UpdatePetPositionHandler handler,
        [FromServices] IValidator<UpdatePetPositionRequest> validator,
        CancellationToken cancellationToken)
    {
        var request = new UpdatePetPositionRequest(volunteerId, petId, dto);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.Handle(request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpGet("{volunteerId:guid}/pet/{petId:guid}/photo-links")]
    public async Task<ActionResult> GetPetPhotoLinks(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] GetPetPhotosHandler getPetPhotosHandler,
        [FromServices] IVolunteersRepository volunteersRepository,
        [FromServices] IValidator<GetPetPhotosRequest> validator,
        CancellationToken cancellationToken)
    {
        var volunteerResult = await volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToResponse();

        var petResult = volunteerResult.Value.GetPetById(petId);
        if (petResult.IsFailure)
            return petResult.Error.ToResponse();

        var request = new GetPetPhotosRequest(volunteerId, petId);

        var result = await getPetPhotosHandler.Handle(request, cancellationToken);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpPost("{volunteerId:guid}/pet/{petId:guid}/photos")]
    public async Task<ActionResult> UploadPetPhotos(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromForm] IFormFileCollection files,
        [FromServices] UploadPetPhotosHandler uploadPetPhotoHandler,
        [FromServices] IValidator<UploadPetPhotosRequest> validator,
        CancellationToken cancellationToken)
    {
        if (files.Count == 0)
            return Errors.General.NoFileForProcessing().ToResponse();

        await using var fileProcessor = new FormFileProcessor();
        var fileDtos = fileProcessor.Process(files);

        var request = new UploadPetPhotosRequest(volunteerId, petId, fileDtos);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await uploadPetPhotoHandler.Handle(request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/photos")]
    public async Task<ActionResult> DeletePetPhotos(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] IEnumerable<string> photoNames,
        [FromServices] DeletePetPhotosHandler deletePetPhotosHandler,
        [FromServices] IValidator<DeletePetPhotosRequest> validator,
        CancellationToken cancellationToken)
    {
        var request = new DeletePetPhotosRequest(volunteerId, petId, photoNames);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await deletePetPhotosHandler.Handle(request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
}
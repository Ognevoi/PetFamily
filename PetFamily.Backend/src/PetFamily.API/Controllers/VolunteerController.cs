using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.DTO.Requests.Volunteer;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.Application.Features.Volunteers;
using PetFamily.Application.Features.Volunteers.Queries.GetPetById;
using PetFamily.Application.Features.Volunteers.Queries.GetVolunteerById;
using PetFamily.Domain.Shared;


namespace PetFamily.API.Controllers;

public class VolunteerController : ApplicationController
{
    private readonly ISender _sender;

    public VolunteerController(ISender sender)
        => _sender = sender;

    [HttpGet]
    public async Task<ActionResult> GetAll(
        [FromQuery] GetVolunteerWithPaginationRequest request,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        var result = await _sender.Send(query, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpGet("{volunteerId:guid}")]
    public async Task<ActionResult> GetById(
        [FromRoute] Guid volunteerId,
        CancellationToken cancellationToken)
    {
        var query = new GetVolunteerByIdQuery(volunteerId);
        var result = await _sender.Send(query, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpGet("pets")]
    public async Task<ActionResult> GetAllPets(
        [FromQuery] GetPetWithPaginationRequest request,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        var result = await _sender.Send(query, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpGet("pets/{petId:guid}")]
    public async Task<ActionResult> GetPetById(
        [FromRoute] Guid petId,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetPetByIdQuery(petId), cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult> Create(
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult> UpdateMainInfo(
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpPut("{id:guid}/social-networks")]
    public async Task<ActionResult> UpdateSocialNetworks(
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerSocialNetworksRequest request,
        [FromServices] IValidator<UpdateVolunteerSocialNetworksRequest> validator,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpPut("{id:guid}/assistance-details")]
    public async Task<ActionResult> UpdateAssistanceDetails(
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerAssistanceDetailsRequest request,
        [FromServices] IValidator<UpdateVolunteerAssistanceDetailsRequest> validator,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpDelete("{id:guid}/soft")]
    public async Task<ActionResult> SoftDelete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new SoftDeleteVolunteerRequest().ToCommand(id);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpDelete("{id:guid}/hard")]
    public async Task<ActionResult> HardDelete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteVolunteerRequest().ToCommand(id);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpPatch("{id:guid}/restore")]
    public async Task<ActionResult> Restore(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new RestoreVolunteerRequest().ToCommand(id);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpPost("{volunteerId:guid}/pet")]
    public async Task<ActionResult> AddPet(
        [FromRoute] Guid volunteerId,
        [FromBody] AddPetRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/hard")]
    public async Task<ActionResult> HardDeletePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        CancellationToken cancellationToken)
    {
        var command = new DeletePetRequest().ToCommand(volunteerId, petId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpPatch("{volunteerId:guid}/pet/{petId:guid}/position")]
    public async Task<ActionResult> UpdatePetPosition(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] UpdatePetPositionRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId, petId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpGet("{volunteerId:guid}/pet/{petId:guid}/photo-links")]
    public async Task<ActionResult> GetPetPhotoLinks(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
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

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpPost("{volunteerId:guid}/pet/{petId:guid}/photos")]
    public async Task<ActionResult> UploadPetPhotos(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromForm] IFormFileCollection files,
        CancellationToken cancellationToken)
    {
        if (files.Count == 0)
            return Errors.General.NoFileForProcessing().ToResponse();

        await using var fileProcessor = new FormFileProcessor();
        var fileDtos = fileProcessor.Process(files);

        var command = new UploadPetPhotosRequest().ToCommand(fileDtos, volunteerId, petId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/photos")]
    public async Task<ActionResult> DeletePetPhotos(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] IEnumerable<string> photoNames,
        CancellationToken cancellationToken)
    {
        var command = new DeletePetPhotosRequest().ToCommand(volunteerId, petId, photoNames);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
}
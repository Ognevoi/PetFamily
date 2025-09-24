using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.DTO.Requests.Species;
using PetFamily.API.Extensions;
using DeleteSpecieRequest = PetFamily.API.DTO.Requests.Species.DeleteSpecieRequest;


namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SpecieController : ControllerBase
{
    private readonly ISender _sender;

    public SpecieController(ISender sender)
        => _sender = sender;

    [HttpPost]
    public async Task<ActionResult> Create(
        [FromBody] CreateSpecieRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpDelete]
    public async Task<ActionResult> Delete(
        [FromBody] DeleteSpecieRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpPost("{id:guid}/breed")]
    public async Task<IActionResult> AddBreed(
        [FromRoute] Guid id,
        [FromBody] AddBreedRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpDelete("{id:guid}/breed/{breedId:guid}")]
    public async Task<IActionResult> DeleteBreed(
        [FromRoute] Guid id,
        [FromRoute] Guid breedId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteBreedRequest().ToCommand(id, breedId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
}
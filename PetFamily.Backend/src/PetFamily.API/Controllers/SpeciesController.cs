using Microsoft.AspNetCore.Mvc;
using PetFamily.API.DTO.Requests.Specie;
using PetFamily.API.Extensions;
using PetFamily.Application.Features.Species.AddBreed;
using PetFamily.Application.Features.Species.CreateSpecie;
using PetFamily.Application.Features.Species.DeleteBreed;
using PetFamily.Application.Features.Species.DeleteSpecie;
using DeleteSpecieRequest = PetFamily.API.DTO.Requests.Specie.DeleteSpecieRequest;


namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SpecieController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromServices] CreateSpecieHandler handler,
        [FromBody] CreateSpecieRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        
        var result = await handler.HandleAsync(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpDelete]
    public async Task<ActionResult> Delete(
        [FromServices] DeleteSpecieHandler handler,
        [FromBody] DeleteSpecieRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        
        var result = await handler.HandleAsync(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpPost("{id:guid}/breed")]
    public async Task<IActionResult> AddBreed(
        [FromRoute] Guid id,
        [FromBody] AddBreedRequest request,
        [FromServices] AddBreedHandler addBreedHandler,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        
        var result = await addBreedHandler.HandleAsync(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpDelete("{id:guid}/breed/{breedId:guid}")]
    public async Task<IActionResult> DeleteBreed(
        [FromRoute] Guid id,
        [FromRoute] Guid breedId,
        [FromServices] DeleteBreedHandler deleteBreedHandler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteBreedRequest().ToCommand(id, breedId);
        
        var result = await deleteBreedHandler.HandleAsync(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
}
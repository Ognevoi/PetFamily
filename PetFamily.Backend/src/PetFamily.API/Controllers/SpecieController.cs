using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Species.AddBreed;
using PetFamily.Application.Species.CreateSpecie;
using PetFamily.Application.Species.DeleteBreed;
using PetFamily.Application.Species.DeleteSpecie;


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
        var result = await handler.Handle(request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpDelete]
    public async Task<ActionResult> Delete(
        [FromServices] DeleteSpecieHandler handler,
        [FromBody] DeleteSpecieRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpPost("{id:guid}/breed")]
    public async Task<IActionResult> AddBreed(
        [FromRoute] Guid id,
        [FromBody] AddBreedDto dto,
        [FromServices] AddBreedHandler addBreedHandler,
        CancellationToken cancellationToken)
    {
        var request = new AddBreedRequest(id, dto);
        var result = await addBreedHandler.Handle(request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpDelete("{id:guid}/breed/{breedId:guid}")]
    public async Task<IActionResult> DeleteBreed(
        [FromRoute] Guid id,
        [FromRoute] Guid breedId,
        [FromServices] DeleteBreedHandler deleteBreedHandler,
        CancellationToken cancellationToken)
    {
        var request = new DeleteBreedRequest(id, breedId);
        var result = await deleteBreedHandler.Handle(request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
}
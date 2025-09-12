using PetFamily.Application.Features.Species.DeleteSpecie;

namespace PetFamily.API.DTO.Requests.Specie;

public record DeleteSpecieRequest(Guid SpecieId);

public static class DeleteSpecieRequestExtensions
{
    public static DeleteSpecieCommand ToCommand(this DeleteSpecieRequest request)
        => new DeleteSpecieCommand(request.SpecieId);
}
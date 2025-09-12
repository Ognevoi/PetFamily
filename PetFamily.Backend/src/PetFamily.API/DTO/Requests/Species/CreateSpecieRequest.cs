using PetFamily.Application.Features.Species.CreateSpecie;

namespace PetFamily.API.DTO.Requests.Specie;

public sealed record CreateSpecieRequest(
    string Name
);

public static class CreateSpecieRequestExtensions
{
    public static CreateSpecieCommand ToCommand(this CreateSpecieRequest request)
        => new CreateSpecieCommand(request.Name);
}
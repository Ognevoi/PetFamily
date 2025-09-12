using PetFamily.Application.Features.Species.AddBreed;

namespace PetFamily.API.DTO.Requests.Specie;

public sealed record AddBreedRequest(
    string Name
);

public static class AddBreedRequestExtensions
{
    public static AddBreedCommand ToCommand(this AddBreedRequest request, Guid SpecieId)
        => new(SpecieId, request.Name);
}

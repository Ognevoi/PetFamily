using PetFamily.Application.Features.Species.DeleteBreed;

namespace PetFamily.API.DTO.Requests.Specie;

public sealed record DeleteBreedRequest();

public static class DeleteBreedRequestExtensions
{
    public static DeleteBreedCommand ToCommand(this DeleteBreedRequest request, Guid SpecieId, Guid BreedId)
        => new(SpecieId, BreedId);
}
using PetFamily.Application.Features.Species.DeleteBreed;

namespace PetFamily.API.DTO.Requests.Species;

public sealed record DeleteBreedRequest
{
    public DeleteBreedCommand ToCommand(Guid specieId, Guid breedId)
        => new(specieId, breedId);
}
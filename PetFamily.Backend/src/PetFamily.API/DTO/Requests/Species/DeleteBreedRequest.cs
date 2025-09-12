using PetFamily.Application.Features.Species.DeleteBreed;

namespace PetFamily.API.DTO.Requests.Specie;

public sealed record DeleteBreedRequest
{
    public DeleteBreedCommand ToCommand(Guid specieId, Guid breedId)
        => new(specieId, breedId);
}
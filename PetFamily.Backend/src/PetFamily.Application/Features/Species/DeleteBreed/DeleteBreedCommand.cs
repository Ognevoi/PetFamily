using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Species.DeleteBreed;

public record DeleteBreedCommand(Guid SpecieId, Guid BreedId) : ICommand<Guid>;
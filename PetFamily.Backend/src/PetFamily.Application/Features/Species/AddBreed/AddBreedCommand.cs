using PetFamily.Application.Interfaces;


namespace PetFamily.Application.Features.Species.AddBreed;

public record AddBreedCommand(
    Guid SpecieId,
    string Name)
    : ICommand<Guid>;
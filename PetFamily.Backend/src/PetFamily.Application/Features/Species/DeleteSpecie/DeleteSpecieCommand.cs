using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Species.DeleteSpecie;

public record DeleteSpecieCommand(Guid SpecieId) : ICommand;

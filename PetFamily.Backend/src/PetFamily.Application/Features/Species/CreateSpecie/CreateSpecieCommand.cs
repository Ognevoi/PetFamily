using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Species.CreateSpecie;

public record CreateSpecieCommand(
    string Name)
    : ICommand;
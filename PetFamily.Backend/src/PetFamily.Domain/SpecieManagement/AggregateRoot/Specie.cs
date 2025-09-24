using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.SpecieManagement.Entities;
using PetFamily.Domain.SpecieManagement.Value_Objects;

namespace PetFamily.Domain.SpecieManagement.AggregateRoot;

public class Specie : Shared.Entity<SpecieId>
{
    private readonly List<Breed> _breeds = [];

    private Specie(SpecieId id, string name) : base(id)
    {
        Name = name;
    }

    public string Name { get; private set; }
    public IReadOnlyList<Breed> Breed => _breeds;

    public static Result<Specie> Create(SpecieId id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Specie>("Specie name should not be empty");

        var specie = new Specie(id, name);

        return specie;
    }

    public Result<Breed, Error> GetBreed(BreedId breedId)
    {
        var breed = _breeds.FirstOrDefault(b => b.Id == breedId);

        if (breed is null)
            return Errors.General.NotFound(breedId);

        return breed;
    }

    public void AddBreed(Breed breed) => _breeds.Add(breed);

    public void RemoveBreed(Breed breed) => _breeds.Remove(breed);
}
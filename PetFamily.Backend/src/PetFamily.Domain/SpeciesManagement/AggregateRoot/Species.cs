using CSharpFunctionalExtensions;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.SpeciesManagement.Value_Objects;

namespace PetFamily.Domain.SpeciesManagement.AggregateRoot;

public class Species : Shared.Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = [];
    
    private Species(SpeciesId id, string name) : base(id)
    {
        Name = name;
    }
    
    public string Name { get; private set; } 
    public IReadOnlyList<Breed> Breed => _breeds;
    
    public static Result<Species> Create(SpeciesId id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Species>("Species name should not be empty");
        
        var species = new Species(id, name);
        
        return species;
    }
    
    public void AddBreed(Breed breed) => _breeds.Add(breed);
}
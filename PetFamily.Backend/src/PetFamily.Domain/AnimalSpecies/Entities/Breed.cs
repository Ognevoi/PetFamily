using PetFamily.Domain.AnimalSpecies.Value_Objects;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.AnimalSpecies.Entities;

public class Breed : Shared.Entity<BreedId>
{
    
    private Breed(BreedId id, string name) : base(id)
    {
        Name = name;
    }
    
    public string Name { get; private set; }
    
    public static Result<Breed> Create(BreedId id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "Breed name should not be empty";
        
        var breed = new Breed(id, name);
        
        return breed;
    }
}


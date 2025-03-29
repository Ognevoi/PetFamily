using CSharpFunctionalExtensions;
using PetFamily.Domain.SpecieManagement.Value_Objects;

namespace PetFamily.Domain.SpecieManagement.Entities;

public class Breed : Shared.Entity<BreedId>
{
    
    public Breed(BreedId id, string name) : base(id)
    {
        Name = name;
    }
    
    public string Name { get; private set; }
    
    public static Result<Breed> Create(BreedId id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Breed>("Breed name should not be empty");
        
        var breed = new Breed(id, name);
        
        return breed;
    }
}
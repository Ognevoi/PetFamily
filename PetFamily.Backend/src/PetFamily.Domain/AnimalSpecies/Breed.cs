using CSharpFunctionalExtensions;

namespace PetFamily.Domain.AnimalSpecies;

public class Breed
{
    
    private Breed(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
    
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    
    public static Result<Breed> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Breed>("Breed name should not be empty");
        
        var breed = new Breed(name);
        
        return Result.Success(breed);
    }
}


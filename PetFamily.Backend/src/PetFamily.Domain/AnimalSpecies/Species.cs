using CSharpFunctionalExtensions;

namespace PetFamily.Domain.AnimalSpecies;

public class Species
{
    private Species(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        Breeds = new List<Breed>();
    }
    
    public Guid Id { get; private set; }
    public string Name { get; private set; } 
    public List<Breed> Breeds { get; private set; }
    // TODO: Make IReadOnlyList
    
    public static Result<Species> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Species>("Species name should not be empty");
        
        var species = new Species(name);
        
        return Result.Success(species);
    }
}


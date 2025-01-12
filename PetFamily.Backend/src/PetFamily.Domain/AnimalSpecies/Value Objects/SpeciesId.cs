namespace PetFamily.Domain.AnimalSpecies.Value_Objects;

public class SpeciesId 
{
    private SpeciesId(Guid value)
    {
        Value = value;
    }
    
    public Guid Value { get; }

    public static SpeciesId NewSpeciesId() => new(Guid.NewGuid());
    
    public static SpeciesId Empty() => new(Guid.Empty);
    public static SpeciesId Create(Guid id) => new(id);
}
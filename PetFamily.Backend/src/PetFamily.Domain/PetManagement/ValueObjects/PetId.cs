using CSharpFunctionalExtensions;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public class PetId : ComparableValueObject
{
    private PetId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static PetId NewPetId() => new(Guid.NewGuid());
    
    public static PetId Empty() => new(Guid.Empty);
    public static PetId Create(Guid id) => new(id);
    
    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
    
    public static implicit operator PetId(Guid id) => new(id);
    public static implicit operator Guid(PetId petId)
    {
        ArgumentNullException.ThrowIfNull(petId);
        return petId.Value;
    }
}
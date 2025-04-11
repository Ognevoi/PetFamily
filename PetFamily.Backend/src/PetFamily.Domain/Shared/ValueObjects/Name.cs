using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared.ValueObjects;

public class Name : ValueObject
{
    private Name(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Name, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired(nameof(Name));

        var name = new Name(value);

        return name;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    { 
        yield return Value;
    }
}
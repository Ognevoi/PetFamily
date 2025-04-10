using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public class Description : ValueObject
{
    private Description(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Description, Error> Create(string description)
    {
        var descriptionValue = new Description(description);

        return descriptionValue;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
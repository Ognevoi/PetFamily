using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public class PetColor : ValueObject
{
    private PetColor(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<PetColor, Error> Create(string petColor)
    {
        if (string.IsNullOrWhiteSpace(petColor))
            return Errors.General.ValueIsRequired("Pet color");

        var petColorValue = new PetColor(petColor);

        return petColorValue;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
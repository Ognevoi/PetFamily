using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public class PetName : ValueObject
{
    private PetName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<PetName, Error> Create(string petName)
    {
        if (string.IsNullOrWhiteSpace(petName))
            return Errors.General.ValueIsRequired("Pet name");

        if (petName.Length > Constants.MAX_VERY_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsTooLong("Pet name", Constants.MAX_VERY_LOW_TEXT_LENGTH);

        var petNameValue = new PetName(petName);

        return petNameValue;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public class IsSterilized : ValueObject
{
    private IsSterilized(bool value)
    {
        Value = value;
    }

    public bool Value { get; }

    public static Result<IsSterilized, Error> Create(bool isSterilized)
    {
        return new IsSterilized(isSterilized);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
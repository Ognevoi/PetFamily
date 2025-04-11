using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public class Height : ValueObject
{
    private Height(double value)
    {
        Value = value;
    }

    public double Value { get; }

    public static Result<Height, Error> Create(double height)
    {
        if (height <= 0)
            return Errors.General.ValueIsInvalid(nameof(Height));

        return new Height(height);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
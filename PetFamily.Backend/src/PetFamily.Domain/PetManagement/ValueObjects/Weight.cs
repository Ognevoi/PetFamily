using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record Weight
{
    private Weight(double value)
    {
        Value = value;
    }

    public double Value { get; }

    public static Result<Weight, Error> Create(double weight)
    {
        if (weight <= 0)
            return Errors.General.ValueIsInvalid("Weight");

        return new Weight(weight);
    }
}
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record IsSterilized
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
}
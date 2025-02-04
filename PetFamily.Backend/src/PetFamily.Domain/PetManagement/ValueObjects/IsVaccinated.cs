using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record IsVaccinated
{
    private IsVaccinated(bool value)
    {
        Value = value;
    }

    public bool Value { get; }

    public static Result<IsVaccinated, Error> Create(bool isVaccinated)
    {
        return new IsVaccinated(isVaccinated);
    }
}
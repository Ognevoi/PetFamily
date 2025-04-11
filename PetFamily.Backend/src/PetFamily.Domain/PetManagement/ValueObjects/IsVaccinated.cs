using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public class IsVaccinated : ValueObject
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

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
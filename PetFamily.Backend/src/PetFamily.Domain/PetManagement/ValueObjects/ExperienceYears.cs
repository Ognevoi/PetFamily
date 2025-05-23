using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public class ExperienceYears : ValueObject
{
    private ExperienceYears(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static Result<ExperienceYears, Error> Create(int experienceYears)
    {
        if (experienceYears < 0)
            return Errors.General.ValueIsInvalid("Experience years");

        return new ExperienceYears(experienceYears);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
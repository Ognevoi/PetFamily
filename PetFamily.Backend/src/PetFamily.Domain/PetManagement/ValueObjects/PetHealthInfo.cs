using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public class PetHealthInfo : ValueObject
{
    private PetHealthInfo(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<PetHealthInfo, Error> Create(string petHealthInfo)
    {
        if (string.IsNullOrWhiteSpace(petHealthInfo))
            return Errors.General.ValueIsRequired("Pet health info");

        var petHealthInfoValue = new PetHealthInfo(petHealthInfo);

        return petHealthInfoValue;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
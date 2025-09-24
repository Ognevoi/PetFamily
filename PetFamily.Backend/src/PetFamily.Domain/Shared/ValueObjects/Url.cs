using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared.ValueObjects;

public class Url : ValueObject
{
    private Url(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Url, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired(nameof(Url));

        if (value.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsTooLong(nameof(Url), Constants.MAX_LOW_TEXT_LENGTH);

        var url = new Url(value);

        return url;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
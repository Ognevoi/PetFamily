using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public class Email : ValueObject
{
    private Email(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Email, Error> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Errors.General.ValueIsRequired(nameof(Email));
        
        if (!IsValidEmail(email))
            return Errors.General.ValueIsInvalid(nameof(Email));

        return new Email(email);
    }

    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        return Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
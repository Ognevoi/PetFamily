using System.Text.RegularExpressions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects;

public record Email
{
    private Email(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Email> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return "Email cannot be empty";
        }

        if (!IsValidEmail(email))
        {
            return "Email is invalid";
        }

        return new Email(email);
    }

    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        return Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase);
    }
}
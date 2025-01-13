using System.Text.RegularExpressions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects;

public record PhoneNumber
{
    private PhoneNumber(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<PhoneNumber> Create(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return "Phone number cannot be empty";
        
        
        if (!Regex.IsMatch(phoneNumber, @"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$"))
        {
            return "Phone number is invalid";
        }

        return new PhoneNumber(phoneNumber);
    }
}
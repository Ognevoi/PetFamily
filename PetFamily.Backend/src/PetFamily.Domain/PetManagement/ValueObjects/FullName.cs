using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public class FullName : ValueObject
{
    private FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public string FirstName { get; }
    public string LastName { get; }

    public static Result<FullName, Error> Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Errors.General.ValueIsRequired("First name");
        
        if (firstName.Length > Constants.MAX_VERY_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsTooLong("First name", Constants.MAX_VERY_LOW_TEXT_LENGTH);

        if (string.IsNullOrWhiteSpace(lastName))
            return Errors.General.ValueIsRequired("Last name");
        
        if (lastName.Length > Constants.MAX_VERY_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsTooLong("Last name", Constants.MAX_VERY_LOW_TEXT_LENGTH);
        
        var fullName = new FullName(firstName, lastName);

        return fullName;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }
}
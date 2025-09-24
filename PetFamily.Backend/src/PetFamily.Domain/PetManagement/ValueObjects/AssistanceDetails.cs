using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public class AssistanceDetails : ValueObject
{
    private AssistanceDetails(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; }
    public string Description { get; }

    public static Result<AssistanceDetails, Error> Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsRequired("Assistance Name");

        if (string.IsNullOrWhiteSpace(description))
            return Errors.General.ValueIsRequired("Assistance Description");

        var assistanceDetails = new AssistanceDetails(name, description);

        return assistanceDetails;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Description;
    }
}
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects;

public record Address
{
    private Address(string street, string city, string state, string zipCode)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
    }

    public string Street { get; }
    public string City { get; }
    public string State { get; }
    public string ZipCode { get; }

    public static Result<Address> Create(string street, string city, string state, string zipCode)
    {
        if (string.IsNullOrWhiteSpace(street))
            return "Street should not be empty";

        if (string.IsNullOrWhiteSpace(city))
            return "City should not be empty";

        if (string.IsNullOrWhiteSpace(state))
            return "State should not be empty";

        if (string.IsNullOrWhiteSpace(zipCode))
            return "Zip code should not be empty";

        var address = new Address(street, city, state, zipCode);

        return address;
    }
}
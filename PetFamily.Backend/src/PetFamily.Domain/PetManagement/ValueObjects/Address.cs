using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public class Address : ValueObject
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

    public static Result<Address, Error> Create(string street, string city, string state, string zipCode)
    {
        if (string.IsNullOrWhiteSpace(street))
            return Errors.General.ValueIsRequired("Street");

        if (string.IsNullOrWhiteSpace(city))
            return Errors.General.ValueIsRequired("City");

        if (string.IsNullOrWhiteSpace(state))
            return Errors.General.ValueIsRequired("State");

        if (string.IsNullOrWhiteSpace(zipCode))
            return Errors.General.ValueIsRequired("ZipCode");

        var address = new Address(street, city, state, zipCode);

        return address;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return ZipCode;
    }
}
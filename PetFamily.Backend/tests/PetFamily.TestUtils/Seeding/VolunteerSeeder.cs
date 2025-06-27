using Bogus;
using PetFamily.Application.Features.Volunteers;
using PetFamily.Application.Features.Volunteers.Commands.DTO;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.Enums;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.SpecieManagement.AggregateRoot;
using PetFamily.Domain.SpecieManagement.Entities;


namespace PetFamily.TestUtils.Seeding;

public static class VolunteerSeeder
{
    public static async Task<Volunteer> SeedVolunteerAsync(IVolunteersRepository context)
    {
        Faker faker = new();

        var volunteerId = VolunteerId.NewVolunteerId();
        var fullName = FullName.Create("Alice", "White").Value;
        var email = Email.Create(faker.Internet.Email()).Value;
        var description = Description.Create("Best volunteer ever!").Value;
        var experience = ExperienceYears.Create(2).Value;
        var phoneNumber = PhoneNumber.Create(faker.Phone.PhoneNumber("###########")).Value;

        var volunteer = Volunteer.Create(
            volunteerId,
            fullName,
            email,
            description,
            experience,
            phoneNumber
        ).Value;

        await context.Add(volunteer);
        await context.Save(volunteer);

        return volunteer;
    }

    public static async Task<Volunteer> SeedSoftDeletedVolunteerAsync(IVolunteersRepository context)
    {
        var volunteer = await SeedVolunteerAsync(context);
        volunteer.SoftDelete();

        await context.Save(volunteer);

        return volunteer;
    }

    public static async Task<Pet> SeedPetAsync(
        IVolunteersRepository context, Volunteer volunteer, Specie specie, Breed breed)
    {
        Faker faker = new();

        var name = faker.Person.FirstName;
        var description = "Friendly and playful.";
        var petColor = faker.Commerce.Color();
        var petHealth = "Healthy";
        var weight = 20;
        var height = 10;
        var address = new AddressDto(faker.Address.StreetAddress(), faker.Address.City(), faker.Address.State(),
            faker.Address.ZipCode());
        var birthDate = faker.Date.Past(5).ToUniversalTime();
        var petStatus = PetStatus.FoundHome;

        var pet = new Pet(
            PetId.NewPetId(),
            PetName.Create(name).Value,
            specie,
            breed,
            Address.Create(address.Street, address.City, address.State, address.ZipCode).Value,
            Description.Create(description).Value,
            PetColor.Create(petColor).Value,
            PetHealthInfo.Create(petHealth).Value,
            Weight.Create(weight).Value,
            Height.Create(height).Value,
            IsSterilized.Create(false).Value,
            IsVaccinated.Create(false).Value,
            birthDate,
            petStatus
        );

        volunteer.AddPet(pet);

        await context.Save(volunteer);

        return pet;
    }
}
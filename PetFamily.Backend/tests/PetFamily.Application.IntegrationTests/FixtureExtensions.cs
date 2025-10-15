using AutoFixture;
using PetFamily.Application.Features.Volunteers.Commands.Create;
using PetFamily.Domain.PetManagement.ValueObjects;
using Bogus;
using PetFamily.Application.Features.Species.AddBreed;
using PetFamily.Application.Features.Species.CreateSpecie;
using PetFamily.Application.Features.Volunteers.Commands.AddPet;
using PetFamily.Application.Features.Volunteers.Commands.DTO;
using PetFamily.Application.Features.Volunteers.Commands.UpdateVolunteerAssistanceDetails;
using PetFamily.Application.Features.Volunteers.Commands.UpdateVolunteerSocialNetworks;
using PetFamily.Domain.PetManagement.Enums;

namespace IntegrationTests;

public static class FixtureExtensions
{
    public static CreateVolunteerCommand BuildCreateVolunteerCommand(
        this IFixture fixture,
        Guid volunteerId = default,
        string? firstName = default,
        string? lastName = default,
        string email = default,
        string? description = default,
        int? experienceYears = default,
        string? phoneNumber = default,
        List<SocialNetworkDto>? socialNetworks = default,
        List<AssistanceDetailsDto>? assistanceDetails = default)
    {
        Faker faker = new();

        // Create default values if not provided
        volunteerId = volunteerId == default ? VolunteerId.NewVolunteerId() : volunteerId;
        firstName ??= "Alice";
        lastName ??= "White";
        email ??= faker.Internet.Email();
        description ??= "Best volunteer ever!";
        experienceYears ??= faker.Random.Int(1, 20);
        phoneNumber ??= faker.Phone.PhoneNumber("###########");
        socialNetworks ??= new List<SocialNetworkDto>
        {
            new(faker.Internet.UserName(), faker.Internet.Url()),
        };
        assistanceDetails ??= new List<AssistanceDetailsDto>
        {
            new(faker.Lorem.Word(), faker.Lorem.Sentence()),
        };

        return fixture.Build<CreateVolunteerCommand>()
            .With(c => c.VolunteerId, VolunteerId.Create(volunteerId))
            .With(c => c.FullName, new FullNameDto(firstName, lastName))
            .With(c => c.Email, () => email)
            .With(c => c.Description, () => description)
            .With(c => c.ExperienceYears, () => experienceYears)
            .With(c => c.PhoneNumber, () => phoneNumber)
            .With(c => c.SocialNetworks, () => socialNetworks)
            .With(c => c.AssistanceDetails, () => assistanceDetails)
            .Create();
    }

    public static AddPetCommand BuildAddPetCommand(
        this IFixture fixture,
        Guid volunteerId = default,
        string name = default,
        Guid specieId = default,
        Guid breedId = default,
        string description = default,
        string petColor = default,
        string petHealth = default,
        double weight = default,
        double height = default,
        bool isSterilized = false,
        bool isVaccinated = false,
        AddressDto address = default,
        DateTime? birthDate = default,
        PetStatus petStatus = default)
    {
        Faker faker = new();

        // Create default values if not provided
        volunteerId = volunteerId == default ? VolunteerId.NewVolunteerId() : volunteerId;
        name ??= "Buddy";
        specieId = specieId == default ? Guid.NewGuid() : specieId;
        breedId = breedId == default ? Guid.NewGuid() : breedId;
        description ??= "Friendly and playful.";
        petColor ??= faker.Commerce.Color();
        petHealth ??= "Healthy";
        weight = weight == 0 ? faker.Random.Double(1, 50) : weight;
        height = height == 0 ? faker.Random.Double(0.5, 2.0) : height;
        address ??= new AddressDto(faker.Address.StreetAddress(), faker.Address.City(), faker.Address.State(),
            faker.Address.ZipCode());
        birthDate ??= faker.Date.Past(5).ToUniversalTime();
        petStatus = petStatus == default ? PetStatus.FoundHome : petStatus;

        return fixture.Build<AddPetCommand>()
            .With(c => c.VolunteerId, volunteerId)
            .With(c => c.Name, name)
            .With(c => c.SpecieId, specieId)
            .With(c => c.BreedId, breedId)
            .With(c => c.Description, description)
            .With(c => c.PetColor, petColor)
            .With(c => c.PetHealth, petHealth)
            .With(c => c.Weight, weight)
            .With(c => c.Height, height)
            .With(c => c.IsSterilized, isSterilized)
            .With(c => c.IsVaccinated, isVaccinated)
            .With(c => c.Address, address)
            .With(c => c.BirthDate, birthDate)
            .With(c => c.PetStatus, petStatus)
            .Create();
    }

    public static CreateSpecieCommand BuildCreateSpecieCommand(
        this IFixture fixture,
        string name = default)
    {
        Faker faker = new();

        // Create default values if not provided
        name ??= "Dog";

        return fixture.Build<CreateSpecieCommand>()
            .With(c => c.Name, name)
            .Create();
    }

    public static AddBreedCommand BuildAddBreedCommand(
        this IFixture fixture,
        Guid specieId = default,
        string name = default)
    {
        Faker faker = new();

        // Create default values if not provided
        specieId = specieId == default ? Guid.NewGuid() : specieId;
        name ??= "Labrador";

        return fixture.Build<AddBreedCommand>()
            .With(c => c.SpecieId, specieId)
            .With(c => c.Name, name)
            .Create();
    }

    public static UpdateVolunteerAssistanceDetailsCommand BuildUpdateVolunteerAssistanceDetailsCommand(
        this IFixture fixture,
        Guid volunteerId = default,
        List<AssistanceDetailsDto>? assistanceDetails = default)
    {
        Faker faker = new();

        // Create default values if not provided
        volunteerId = volunteerId == default ? VolunteerId.NewVolunteerId() : volunteerId;
        assistanceDetails ??= new List<AssistanceDetailsDto>
        {
            new(faker.Lorem.Word(), faker.Lorem.Sentence()),
        };

        return fixture.Build<UpdateVolunteerAssistanceDetailsCommand>()
            .With(c => c.VolunteerId, volunteerId)
            .With(c => c.AssistanceDetails, assistanceDetails)
            .Create();
    }

    public static UpdateVolunteerSocialNetworksCommand BuildUpdateVolunteerSocialNetworksCommand(
        this IFixture fixture,
        Guid volunteerId = default,
        List<SocialNetworkDto>? socialNetworks = default)
    {
        Faker faker = new();

        // Create default values if not provided
        volunteerId = volunteerId == default ? VolunteerId.NewVolunteerId() : volunteerId;
        socialNetworks ??= new List<SocialNetworkDto>
        {
            new(faker.Internet.UserName(), faker.Internet.Url()),
        };

        return fixture.Build<UpdateVolunteerSocialNetworksCommand>()
            .With(c => c.VolunteerId, volunteerId)
            .With(c => c.SocialNetworks, socialNetworks)
            .Create();
    }
}
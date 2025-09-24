using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.Volunteers.Queries.GetPets;
using PetFamily.TestUtils.Seeding;

namespace IntegrationTests.Volunteers;

public class GetPetsHandlerTests : VolunteerTestBase
{
    private readonly ISender _sender;

    public GetPetsHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sender = Scope.ServiceProvider.GetRequiredService<ISender>();
    }

    [Theory]
    [InlineData(true, true, true)]
    [InlineData(true, true, false)]
    [InlineData(true, false, true)]
    [InlineData(true, true, null)]
    [InlineData(true, null, true)]
    [InlineData(null, true, true)]
    public async Task GetPets_ShouldReturnPets_WhenQueryIsValid(
        bool? isValidVolunteerId,
        bool? isValidName,
        bool? isValidAge
    )
    {
        // Arrange
        var specie = await SpecieSeeder.SeedSpecieAsync(SpecieRepository);
        var breed = await SpecieSeeder.SeedBreedAsync(SpecieRepository, specie);

        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(VolunteerRepository);
        var pet = await VolunteerSeeder.SeedPetAsync(VolunteerRepository, volunteer, specie, breed);


        Guid? volunteerId = isValidVolunteerId == null
            ? null
            : (isValidVolunteerId == true ? volunteer.Id : Guid.NewGuid());
        string? name = isValidName == null ? null : (isValidName == true ? pet.Name.Value : "NonExistentName");
        int? age = isValidAge == null ? null : (isValidAge == true ? DateTime.Now.Year - pet.BirthDate?.Year : -1);

        var query = new GetPetWithPaginationQuery(1, 1, volunteerId, breed.Id, specie.Id, name, age, "name", "asc");

        // Act
        var result = await _sender.Send(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        if (isValidVolunteerId == false || isValidName == false || isValidAge == false)
            result.Value.Items.Should().BeEmpty();
        else
        {
            result.Value.Items.Should().ContainSingle();
            result.Value.Items.First().Id.Should().Be(pet.Id);
        }
    }

    // [Fact]
    // public async Task GetPets_ShouldReturnEmptyList_WhenVolunteerIdIsSoftDeleted()
    // {
    //     // Arrange
    //     var specie = await SpecieSeeder.SeedSpecieAsync(SpecieRepository);
    //     var breed = await SpecieSeeder.SeedBreedAsync(SpecieRepository, specie);
    //
    //     var volunteer = await VolunteerSeeder.SeedSoftDeletedVolunteerAsync(VolunteerRepository);
    //     var pet = await VolunteerSeeder.SeedPetAsync(VolunteerRepository, volunteer, specie, breed);
    //
    //     var query = new GetPetWithPaginationQuery(1, 1, volunteer.Id, breed.Id, specie.Id, pet.Name.Value,
    //         DateTime.Now.Year - pet.BirthDate?.Year, "name", "asc");
    //
    //     // Act
    //     var result = await _sender.Send(query, CancellationToken.None);
    //
    //     // Assert
    //     result.IsSuccess.Should().BeTrue();
    //     result.Value.Items.Should().BeEmpty();
    // }


    [Theory]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    public async Task GetPets_ShouldReturnSortedPets_WhenQueryIsValid(string sortBy, string sortDirection)
    {
        // Arrange
        var specie = await SpecieSeeder.SeedSpecieAsync(SpecieRepository);
        var breed = await SpecieSeeder.SeedBreedAsync(SpecieRepository, specie);

        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(VolunteerRepository);
        await VolunteerSeeder.SeedPetAsync(VolunteerRepository, volunteer, specie, breed);
        await VolunteerSeeder.SeedPetAsync(VolunteerRepository, volunteer, specie, breed);
        await VolunteerSeeder.SeedPetAsync(VolunteerRepository, volunteer, specie, breed);
        await VolunteerSeeder.SeedPetAsync(VolunteerRepository, volunteer, specie, breed);
        await VolunteerSeeder.SeedPetAsync(VolunteerRepository, volunteer, specie, breed);

        var query = new GetPetWithPaginationQuery(1, 10, volunteer.Id, breed.Id, specie.Id, null, null, sortBy,
            sortDirection);

        // Act
        var result = await _sender.Send(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Should().HaveCountGreaterThanOrEqualTo(2);

        if (sortDirection == "asc")
            result.Value.Items.Should().BeInAscendingOrder(p => p.Name);
        else
            result.Value.Items.Should().BeInDescendingOrder(p => p.Name);
    }
}
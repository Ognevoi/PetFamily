using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.Volunteers.Commands.DTO;
using PetFamily.TestUtils.Seeding;

namespace IntegrationTests.Volunteers;

public class AddPetHandlerTests : VolunteerTestBase
{
    private readonly ISender _sender;

    public AddPetHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sender = Scope.ServiceProvider.GetRequiredService<ISender>();
    }

    [Fact]
    public async Task HandleAsync_ShouldAddPetToVolunteer_WhenCommandIsValid()
    {
        //Arrange
        var specie = await SpecieSeeder.SeedSpecieAsync(SpecieRepository);
        var breed = await SpecieSeeder.SeedBreedAsync(SpecieRepository, specie);

        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(VolunteerRepository);
        var command = Fixture.BuildAddPetCommand(
            volunteerId: volunteer.Id,
            specieId: specie.Id,
            breedId: breed.Id);

        // Act
        var result = await _sender.Send(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        ReadDbContext.Volunteers.Include(v => v.Pets).FirstOrDefault(v => v.Id == volunteer.Id)?.Pets.Should()
            .NotBeEmpty();
    }

    [Theory]
    [InlineData(false, true)]
    [InlineData(true, false)]
    public async Task HandleAsync_ShouldReturnError_WhenVolunteerOrBreedAreInvalid(
        bool useValidVolunteerId,
        bool useValidBreedId)
    {
        // Arrange
        var specie = await SpecieSeeder.SeedSpecieAsync(SpecieRepository);
        var breed = await SpecieSeeder.SeedBreedAsync(SpecieRepository, specie);
        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(VolunteerRepository);

        var command = Fixture.BuildAddPetCommand(
            volunteerId: useValidVolunteerId ? volunteer.Id : Guid.NewGuid(),
            specieId: specie.Id,
            breedId: useValidBreedId ? breed.Id : Guid.NewGuid());

        // Act
        var result = await _sender.Send(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNullOrEmpty();
    }

    [Theory]
    [InlineData("", default, default, default, default, default, default, default, default)]
    [InlineData(default, "", default, default, default, default, default, default, default)]
    [InlineData(default, default, "", default, default, default, default, default, default)]
    [InlineData(default, default, default, -1, default, default, default, default, default)]
    [InlineData(default, default, default, default, -1, default, default, default, default)]
    [InlineData(default, default, default, default, default, "", default, default, default)]
    [InlineData(default, default, default, default, default, default, "", default, default)]
    [InlineData(default, default, default, default, default, default, default, "", default)]
    [InlineData(default, default, default, default, default, default, default, default, "")]
    public async Task HandleAsync_ShouldReturnError_WhenCommandIsInvalid(
        string name,
        string petColor,
        string petHealth,
        double weight,
        double height,
        string addressStreet,
        string addressCity,
        string addressState,
        string addressPostalCode)
    {
        //Arrange
        var specie = await SpecieSeeder.SeedSpecieAsync(SpecieRepository);
        var breed = await SpecieSeeder.SeedBreedAsync(SpecieRepository, specie);

        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(VolunteerRepository);

        var address = new AddressDto(
            addressStreet ?? "123 Main St",
            addressCity ?? "Springfield",
            addressState ?? "IL",
            addressPostalCode ?? "62704");

        var command = Fixture.BuildAddPetCommand(
            volunteerId: volunteer.Id,
            specieId: specie.Id,
            breedId: breed.Id,
            name: name,
            petColor: petColor,
            petHealth: petHealth,
            weight: weight,
            height: height,
            address: address);

        // Act
        var result = await _sender.Send(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNullOrEmpty();
    }
}
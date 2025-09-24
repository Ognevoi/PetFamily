using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.Volunteers.Queries.GetPetById;
using PetFamily.TestUtils.Seeding;

namespace IntegrationTests.Volunteers;

public class GetPetByIdHandlerTests : VolunteerTestBase
{
    private readonly ISender _sender;

    public GetPetByIdHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sender = Scope.ServiceProvider.GetRequiredService<ISender>();
    }

    [Fact]
    public async Task GetPetById_ShouldReturnPet_WhenPetExists()
    {
        // Arrange
        var specie = await SpecieSeeder.SeedSpecieAsync(SpecieRepository);
        var breed = await SpecieSeeder.SeedBreedAsync(SpecieRepository, specie);

        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(VolunteerRepository);
        var pet = await VolunteerSeeder.SeedPetAsync(VolunteerRepository, volunteer, specie, breed);

        // Act
        var result = await _sender.Send(new GetPetByIdQuery(pet.Id), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(pet.Id);

        ReadDbContext.Pets.FirstOrDefault(p => p.Id == pet.Id).Should().NotBeNull();
    }
}
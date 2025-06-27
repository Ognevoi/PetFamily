using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.Volunteers.Commands.UpdatePetPosition;
using PetFamily.Application.Interfaces;
using PetFamily.TestUtils.Seeding;

namespace IntegrationTests.Volunteers;

public class UpdatePetPositionHandlerTests : VolunteerTestBase
{
    private readonly ICommandHandler<Guid, UpdatePetPositionCommand> _sut;

    public UpdatePetPositionHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdatePetPositionCommand>>();
    }

    [Fact]
    public async Task HandleAsync_ShouldUpdatePetPosition_WhenCommandIsValid()
    {
        // Arrange
        var specie = await SpecieSeeder.SeedSpecieAsync(SpecieRepository);
        var breed = await SpecieSeeder.SeedBreedAsync(SpecieRepository, specie);

        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(VolunteerRepository);

        var pet1 = await VolunteerSeeder.SeedPetAsync(VolunteerRepository, volunteer, specie, breed);
        var pet2 = await VolunteerSeeder.SeedPetAsync(VolunteerRepository, volunteer, specie, breed);

        var command = new UpdatePetPositionCommand(volunteer.Id.Value, pet2.Id.Value, 1); // Move pet2 to position 1

        // Act
        var result = await _sut.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(volunteer.Id.Value);

        ReadDbContext.Pets.FirstOrDefault(p => p.Id == pet1.Id.Value && p.Position == 2).Should().NotBeNull();
        ReadDbContext.Pets.FirstOrDefault(p => p.Id == pet2.Id.Value && p.Position == 1).Should().NotBeNull();
    }
}
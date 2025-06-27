using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.Volunteers.Commands.GetPetPhoto;
using PetFamily.Application.Interfaces;
using PetFamily.TestUtils.Seeding;

namespace IntegrationTests.Volunteers;

public class GetPetPhotosHandlerTests : VolunteerTestBase
{
    private readonly ICommandHandler<IEnumerable<string>, GetPetPhotosCommand> _sut;

    public GetPetPhotosHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<IEnumerable<string>, GetPetPhotosCommand>>();
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnPetPhotos_WhenCommandIsValid()
    {
        // Arrange
        var specie = await SpecieSeeder.SeedSpecieAsync(SpecieRepository);
        var breed = await SpecieSeeder.SeedBreedAsync(SpecieRepository, specie);

        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(VolunteerRepository);
        var pet = await VolunteerSeeder.SeedPetAsync(VolunteerRepository, volunteer, specie, breed);

        var command = new GetPetPhotosCommand(volunteer.Id.Value, pet.Id.Value);

        Factory.SetupFileProviderSuccessGetMock();

        // Act
        var result = await _sut.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().ContainSingle().Which.Should().Be("photo1.png");
    }
}
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.Volunteers.Commands.GetPetPhoto;
using PetFamily.TestUtils.Seeding;

namespace IntegrationTests.Volunteers;

public class GetPetPhotosHandlerTests : VolunteerTestBase
{
    private readonly ISender _sender;

    public GetPetPhotosHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sender = Scope.ServiceProvider.GetRequiredService<ISender>();
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
        var result = await _sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().ContainSingle().Which.Should().Be("photo1.png");
    }
}
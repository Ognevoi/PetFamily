using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.Volunteers.Commands.DTO;
using PetFamily.Application.Features.Volunteers.Commands.UploadPetPhoto;
using PetFamily.TestUtils.Seeding;

namespace IntegrationTests.Volunteers;

public class UploadPetPhotosHandlerTests : VolunteerTestBase
{
    private readonly ISender _sender;

    public UploadPetPhotosHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sender = Scope.ServiceProvider.GetRequiredService<ISender>();
    }

    [Fact]
    public async Task HandleAsync_ShouldUploadPhotos_WhenCommandIsValid()
    {
        // Arrange
        var specie = await SpecieSeeder.SeedSpecieAsync(SpecieRepository);
        var breed = await SpecieSeeder.SeedBreedAsync(SpecieRepository, specie);

        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(VolunteerRepository);
        var pet = await VolunteerSeeder.SeedPetAsync(VolunteerRepository, volunteer, specie, breed);

        var command = new UploadPetPhotosCommand(volunteer.Id.Value, pet.Id.Value,
            new[] { new UploadFileDto(Stream.Null, "photo1.png") });

        Factory.SetupFileProviderSuccessUploadMock();

        // Act
        var result = await _sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        ReadDbContext.Pets.FirstOrDefault().Photos.Should().HaveCount(1);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnError_WhenVolunteerDoesNotExist()
    {
        // Arrange
        var specie = await SpecieSeeder.SeedSpecieAsync(SpecieRepository);
        var breed = await SpecieSeeder.SeedBreedAsync(SpecieRepository, specie);

        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(VolunteerRepository);
        var pet = await VolunteerSeeder.SeedPetAsync(VolunteerRepository, volunteer, specie, breed);

        var command = new UploadPetPhotosCommand(volunteer.Id.Value, pet.Id.Value,
            new[] { new UploadFileDto(Stream.Null, "photo1.png") });

        Factory.SetupFileProviderFailedUploadMock();

        // Act
        var result = await _sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeFalse();

        ReadDbContext.Pets.FirstOrDefault().Photos.Should().HaveCount(0);
    }
}
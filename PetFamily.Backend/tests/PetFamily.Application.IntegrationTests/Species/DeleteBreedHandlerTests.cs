using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.Species.DeleteBreed;
using PetFamily.Domain.SpecieManagement.Value_Objects;
using PetFamily.TestUtils.Seeding;

namespace IntegrationTests.Species;

public class DeleteBreedHandlerTests : SpecieTestBase
{
    private readonly ISender _sender;

    public DeleteBreedHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sender = Scope.ServiceProvider.GetRequiredService<ISender>();
    }

    [Fact]
    public async Task HandleAsync_ShouldDeleteBreed_WhenCommandIsValid()
    {
        // Arrange
        var specie = await SpecieSeeder.SeedSpecieAsync(SpecieRepository);
        var breed = await SpecieSeeder.SeedBreedAsync(SpecieRepository, specie);
        var command = new DeleteBreedCommand(specie.Id, breed.Id);

        // Act
        var result = await _sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(breed.Id.Value);

        ReadDbContext.Breeds.FirstOrDefault().Should().BeNull();
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnError_WhenBreedDoesNotExist()
    {
        // Arrange
        var specie = await SpecieSeeder.SeedSpecieAsync(SpecieRepository);
        var command = new DeleteBreedCommand(specie.Id, BreedId.NewBreedId());

        // Act
        var result = await _sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();

        ReadDbContext.Breeds.FirstOrDefault().Should().BeNull();
    }
}
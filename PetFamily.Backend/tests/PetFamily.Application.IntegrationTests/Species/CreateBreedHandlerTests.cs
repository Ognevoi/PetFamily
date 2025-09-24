using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.TestUtils.Seeding;

namespace IntegrationTests.Species;

public class CreateBreedHandlerTests : SpecieTestBase
{
    private readonly ISender _sender;

    public CreateBreedHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sender = Scope.ServiceProvider.GetRequiredService<ISender>();
    }

    [Fact]
    public async Task HandleAsync_ShouldCreateBreed_WhenCommandIsValid()
    {
        // Arrange
        var specie = await SpecieSeeder.SeedSpecieAsync(SpecieRepository);
        var command = Fixture.BuildAddBreedCommand(specie.Id);

        // Act
        var result = await _sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        ReadDbContext.Breeds.FirstOrDefault().Should().NotBeNull();
    }
}
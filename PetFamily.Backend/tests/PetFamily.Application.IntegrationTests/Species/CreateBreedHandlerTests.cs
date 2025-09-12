using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.Species.AddBreed;
using PetFamily.Application.Interfaces;
using PetFamily.TestUtils.Seeding;

namespace IntegrationTests.Species;

public class CreateBreedHandlerTests : SpecieTestBase
{
    private readonly ICommandHandler<Guid, AddBreedCommand> _sut;

    public CreateBreedHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, AddBreedCommand>>();
    }

    [Fact]
    public async Task HandleAsync_ShouldCreateBreed_WhenCommandIsValid()
    {
        // Arrange
        var specie = await SpecieSeeder.SeedSpecieAsync(SpecieRepository);
        var command = Fixture.BuildAddBreedCommand(specie.Id);

        // Act
        var result = await _sut.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        
        ReadDbContext.Breeds.FirstOrDefault().Should().NotBeNull();
    }

}
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.Species.DeleteSpecie;
using PetFamily.TestUtils.Seeding;

namespace IntegrationTests.Species;

public class DeleteSpecieHandlerTests : SpecieTestBase
{
    private readonly ISender _sender;

    public DeleteSpecieHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sender = Scope.ServiceProvider.GetRequiredService<ISender>();
    }

    [Fact]
    public async Task HandleAsync_ShouldDeleteSpecie_WhenSpecieExists()
    {
        // Arrange
        var specie = await SpecieSeeder.SeedSpecieAsync(SpecieRepository);
        var command = new DeleteSpecieCommand(specie.Id.Value);

        // Act
        var result = await _sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(specie.Id.Value);

        ReadDbContext.Species.FirstOrDefault().Should().BeNull();
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnError_WhenSpecieDoesNotExist()
    {
        // Arrange
        var nonExistentSpecieId = Guid.NewGuid();
        var command = new DeleteSpecieCommand(nonExistentSpecieId);

        // Act
        var result = await _sender.Send(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();

        ReadDbContext.Species.FirstOrDefault().Should().BeNull();
    }
}
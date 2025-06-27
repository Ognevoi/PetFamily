using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.Species.CreateSpecie;
using PetFamily.Application.Interfaces;

namespace IntegrationTests.Species;

public class CreateSpecieHandlerTests : SpecieTestBase
{
    private readonly ICommandHandler<Guid, CreateSpecieCommand> _sut;

    public CreateSpecieHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateSpecieCommand>>();
    }

    [Fact]
    public async Task HandleAsync_ShouldCreateSpecie_WhenCommandIsValid()
    {
        // Arrange
        var command = Fixture.BuildCreateSpecieCommand();

        // Act
        var result = await _sut.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        ReadDbContext.Species.FirstOrDefault().Should().NotBeNull();
    }

}
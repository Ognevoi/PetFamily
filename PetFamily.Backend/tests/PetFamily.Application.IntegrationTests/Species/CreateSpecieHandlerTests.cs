using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Species;

public class CreateSpecieHandlerTests : SpecieTestBase
{
    private readonly ISender _sender;

    public CreateSpecieHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sender = Scope.ServiceProvider.GetRequiredService<ISender>();
    }

    [Fact]
    public async Task HandleAsync_ShouldCreateSpecie_WhenCommandIsValid()
    {
        // Arrange
        var command = Fixture.BuildCreateSpecieCommand();

        // Act
        var result = await _sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        ReadDbContext.Species.FirstOrDefault().Should().NotBeNull();
    }
}
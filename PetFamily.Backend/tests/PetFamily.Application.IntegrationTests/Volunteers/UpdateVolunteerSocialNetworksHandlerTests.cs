using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.TestUtils.Seeding;

namespace IntegrationTests.Volunteers;

public class UpdateVolunteerSocialNetworksHandlerTests : VolunteerTestBase
{
    private readonly ISender _sender;

    public UpdateVolunteerSocialNetworksHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sender = Scope.ServiceProvider.GetRequiredService<ISender>();
    }

    [Fact]
    public async Task HandleAsync_ShouldUpdateVolunteerSocialNetworks_WhenCommandIsValid()
    {
        // Arrange
        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(VolunteerRepository);
        var command = Fixture.BuildUpdateVolunteerSocialNetworksCommand(volunteer.Id);

        // Act
        var result = await _sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(volunteer.Id.Value);
    }
}
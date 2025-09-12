using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.Volunteers.Commands.UpdateVolunteerSocialNetworks;
using PetFamily.Application.Interfaces;
using PetFamily.TestUtils.Seeding;

namespace IntegrationTests.Volunteers;

public class UpdateVolunteerSocialNetworksHandlerTests : VolunteerTestBase
{
    private readonly ICommandHandler<Guid, UpdateVolunteerSocialNetworksCommand> _sut;

    public UpdateVolunteerSocialNetworksHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdateVolunteerSocialNetworksCommand>>();
    }

    [Fact]
    public async Task HandleAsync_ShouldUpdateVolunteerSocialNetworks_WhenCommandIsValid()
    {
        // Arrange
        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(VolunteerRepository);
        var command = Fixture.BuildUpdateVolunteerSocialNetworksCommand(volunteer.Id);

        // Act
        var result = await _sut.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(volunteer.Id.Value);
    }
}
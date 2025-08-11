using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.Volunteers.Commands.Restore;
using PetFamily.Application.Interfaces;
using PetFamily.TestUtils.Seeding;

namespace IntegrationTests.Volunteers;

public class RestoreVolunteerHandlerTests : VolunteerTestBase
{
    private readonly ICommandHandler<Guid, RestoreVolunteerCommand> _sut;

    public RestoreVolunteerHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, RestoreVolunteerCommand>>();
    }

    [Fact]
    public async Task HandleAsync_ShouldRestoreVolunteer_WhenVolunteerExists()
    {
        // Arrange
        var volunteer = await VolunteerSeeder.SeedSoftDeletedVolunteerAsync(VolunteerRepository);
        var command = new RestoreVolunteerCommand(volunteer.Id.Value);

        // Act
        var result = await _sut.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(volunteer.Id.Value);

        ReadDbContext.Volunteers.FirstOrDefault(v => v.Id == volunteer.Id).Should().NotBeNull();
    }
}
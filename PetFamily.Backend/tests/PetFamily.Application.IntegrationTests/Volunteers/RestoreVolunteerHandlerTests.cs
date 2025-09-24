using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.Volunteers.Commands.Restore;
using PetFamily.TestUtils.Seeding;

namespace IntegrationTests.Volunteers;

public class RestoreVolunteerHandlerTests : VolunteerTestBase
{
    private readonly ISender _sender;

    public RestoreVolunteerHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sender = Scope.ServiceProvider.GetRequiredService<ISender>();
    }

    [Fact]
    public async Task HandleAsync_ShouldRestoreVolunteer_WhenVolunteerExists()
    {
        // Arrange
        var volunteer = await VolunteerSeeder.SeedSoftDeletedVolunteerAsync(VolunteerRepository);
        var command = new RestoreVolunteerCommand(volunteer.Id.Value);

        // Act
        var result = await _sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(volunteer.Id.Value);

        ReadDbContext.Volunteers.FirstOrDefault(v => v.Id == volunteer.Id).Should().NotBeNull();
    }
}
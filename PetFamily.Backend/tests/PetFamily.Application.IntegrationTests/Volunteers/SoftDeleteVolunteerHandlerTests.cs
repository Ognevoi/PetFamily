using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.Volunteers.Commands.SoftDelete;
using PetFamily.TestUtils.Seeding;

namespace IntegrationTests.Volunteers;

public class SoftDeleteVolunteerHandlerTests : VolunteerTestBase
{
    private readonly ISender _sender;

    public SoftDeleteVolunteerHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sender = Scope.ServiceProvider.GetRequiredService<ISender>();
    }

    [Fact]
    public async Task HandleAsync_ShouldSoftDeleteVolunteer_WhenVolunteerExists()
    {
        // Arrange
        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(VolunteerRepository);
        var command = new SoftDeleteVolunteerCommand(volunteer.Id.Value);

        // Act
        var result = await _sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(volunteer.Id.Value);

        ReadDbContext.Volunteers.IgnoreQueryFilters()
            .FirstOrDefault(v => v.Id == volunteer.Id && v.IsDeleted)
            .Should().NotBeNull();
    }
}
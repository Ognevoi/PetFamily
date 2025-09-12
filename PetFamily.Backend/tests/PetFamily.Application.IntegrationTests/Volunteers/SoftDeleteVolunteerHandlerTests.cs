using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.Volunteers.Commands.SoftDelete;
using PetFamily.Application.Interfaces;
using PetFamily.TestUtils.Seeding;

namespace IntegrationTests.Volunteers;

public class SoftDeleteVolunteerHandlerTests : VolunteerTestBase
{
    private readonly ICommandHandler<Guid, SoftDeleteVolunteerCommand> _sut;

    public SoftDeleteVolunteerHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, SoftDeleteVolunteerCommand>>();
    }

    [Fact]
    public async Task HandleAsync_ShouldSoftDeleteVolunteer_WhenVolunteerExists()
    {
        // Arrange
        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(VolunteerRepository);
        var command = new SoftDeleteVolunteerCommand(volunteer.Id.Value);

        // Act
        var result = await _sut.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(volunteer.Id.Value);

        ReadDbContext.Volunteers.IgnoreQueryFilters()
            .FirstOrDefault(v => v.Id == volunteer.Id && v.IsDeleted)
            .Should().NotBeNull();
    }
}
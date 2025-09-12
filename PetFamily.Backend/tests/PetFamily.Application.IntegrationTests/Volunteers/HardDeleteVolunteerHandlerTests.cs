using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.Volunteers.Commands.HardDelete;
using PetFamily.Application.Interfaces;
using PetFamily.TestUtils.Seeding;

namespace IntegrationTests.Volunteers;

public class HardDeleteVolunteerHandlerTests : VolunteerTestBase
{
    private readonly ICommandHandler<Guid, DeleteVolunteerCommand> _sut;

    public HardDeleteVolunteerHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, DeleteVolunteerCommand>>();
    }

    [Fact]
    public async Task HandleAsync_ShouldHardDeleteVolunteer_WhenVolunteerExists()
    {
        // Arrange
        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(VolunteerRepository);
        var command = new DeleteVolunteerCommand(volunteer.Id.Value);

        // Act
        var result = await _sut.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(volunteer.Id.Value);

        ReadDbContext.Volunteers.FirstOrDefault(v => v.Id == volunteer.Id).Should().BeNull();
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnError_WhenVolunteerDoesNotExist()
    {
        // Arrange
        var nonExistentVolunteerId = Guid.NewGuid();
        var command = new DeleteVolunteerCommand(nonExistentVolunteerId);

        // Act
        var result = await _sut.HandleAsync(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();

        ReadDbContext.Volunteers.FirstOrDefault(v => v.Id == nonExistentVolunteerId).Should().BeNull();
    }
}
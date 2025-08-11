using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.Volunteers.Commands.DTO;
using PetFamily.Application.Features.Volunteers.Commands.Update;
using PetFamily.Application.Interfaces;
using PetFamily.TestUtils.Seeding;

namespace IntegrationTests.Volunteers;

public class UpdateVolunteerHandlerTests : VolunteerTestBase
{
    private readonly ICommandHandler<Guid, UpdateVolunteerCommand> _sut;

    public UpdateVolunteerHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdateVolunteerCommand>>();
    }

    [Fact]
    public async Task HandleAsync_ShouldSuccessfullyUpdateVolunteerFields_WhenCommandIsValid()
    {
        // Arrange
        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(VolunteerRepository);
        var command = new UpdateVolunteerCommand(
            volunteer.Id.Value,
            new FullNameDto("UpdatedFirstName", "UpdatedLastName"),
            "UpdatedEmail@test.com",
            "Updated description",
            5,
            "1234567890"
        );

        // Act
        var result = await _sut.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(volunteer.Id.Value);

        var updatedVolunteer = await ReadDbContext.Volunteers
            .FirstOrDefaultAsync(v => v.Id == volunteer.Id.Value);

        updatedVolunteer.Should().NotBeNull();
        updatedVolunteer.Should().BeEquivalentTo(new
        {
            FirstName = "UpdatedFirstName",
            LastName = "UpdatedLastName",
            Email = "UpdatedEmail@test.com",
            Description = "Updated description",
            ExperienceYears = 5,
            PhoneNumber = "1234567890"
        }, options => options.ExcludingMissingMembers());
    }
}
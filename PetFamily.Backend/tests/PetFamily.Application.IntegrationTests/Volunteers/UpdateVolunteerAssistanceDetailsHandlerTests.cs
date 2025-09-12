using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.Volunteers.Commands.UpdateVolunteerAssistanceDetails;
using PetFamily.Application.Interfaces;
using PetFamily.TestUtils.Seeding;

namespace IntegrationTests.Volunteers;

public class UpdateVolunteerAssistanceDetailsHandlerTests : VolunteerTestBase
{
    private readonly ICommandHandler<Guid, UpdateVolunteerAssistanceDetailsCommand> _sut;

    public UpdateVolunteerAssistanceDetailsHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, UpdateVolunteerAssistanceDetailsCommand>>();
    }

    [Fact]
    public async Task HandleAsync_ShouldUpdateVolunteerAssistanceDetails_WhenCommandIsValid()
    {
        // Arrange
        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(VolunteerRepository);
        var command = Fixture.BuildUpdateVolunteerAssistanceDetailsCommand(volunteer.Id);

        // Act
        var result = await _sut.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(volunteer.Id.Value);
    }
}
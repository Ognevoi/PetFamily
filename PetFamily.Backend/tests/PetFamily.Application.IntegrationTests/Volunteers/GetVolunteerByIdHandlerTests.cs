using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.Volunteers.Queries.GetVolunteerById;
using PetFamily.TestUtils.Seeding;

namespace IntegrationTests.Volunteers;

public class GetVolunteerByIdHandlerTests : VolunteerTestBase
{
    private readonly ISender _sender;

    public GetVolunteerByIdHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sender = Scope.ServiceProvider.GetRequiredService<ISender>();
    }

    [Fact]
    public async Task GetVolunteerById_ShouldReturnVolunteer_WhenExists()
    {
        // Arrange
        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(VolunteerRepository);

        var query = new GetVolunteerByIdQuery(volunteer.Id);

        // Act
        var result = await _sender.Send(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(volunteer.Id);

        ReadDbContext.Volunteers.FirstOrDefault().Should().NotBeNull();
    }

    [Fact]
    public async Task GetVolunteerById_ShouldReturnNotFound_WhenDoesNotExist()
    {
        // Arrange
        var query = new GetVolunteerByIdQuery(Guid.NewGuid());

        // Act
        var result = await _sender.Send(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();

        ReadDbContext.Volunteers.FirstOrDefault().Should().BeNull();
    }
}
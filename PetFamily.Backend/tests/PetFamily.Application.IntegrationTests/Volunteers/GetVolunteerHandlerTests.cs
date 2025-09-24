using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.Volunteers.Queries.GetVolunteers;
using PetFamily.TestUtils.Seeding;

namespace IntegrationTests.Volunteers;

public class GetVolunteersHandlerTests : VolunteerTestBase
{
    private readonly ISender _sender;

    public GetVolunteersHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sender = Scope.ServiceProvider.GetRequiredService<ISender>();
    }

    [Theory]
    [InlineData(true, null)]
    [InlineData(null, true)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(null, null)]
    public async Task GetVolunteers_ShouldReturnVolunteers_WhenQueryIsValid(bool? isValidVolunteerId, bool? isValidName)
    {
        // Arrange
        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(VolunteerRepository);

        Guid? volunteerId = isValidVolunteerId == null
            ? null
            : (isValidVolunteerId == true ? volunteer.Id : Guid.NewGuid());
        string? name = isValidName == null
            ? null
            : (isValidName == true ? volunteer.FullName.FirstName : "NonExistentName");

        var query = new GetVolunteerWithPaginationQuery(1, 1, volunteerId, name);

        // Act
        var result = await _sender.Send(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        if (isValidVolunteerId == false || isValidName == false)
            result.Value.Items.Should().BeEmpty();
        else
            result.Value.Items.Should().ContainSingle();
    }
}
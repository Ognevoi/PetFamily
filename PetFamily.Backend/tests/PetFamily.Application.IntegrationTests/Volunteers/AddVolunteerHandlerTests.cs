using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.Volunteers.Commands.DTO;
using PetFamily.Domain.PetManagement.ValueObjects;

namespace IntegrationTests.Volunteers;

public class AddVolunteerHandlerTests : VolunteerTestBase
{
    private readonly ISender _sender;

    public AddVolunteerHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sender = Scope.ServiceProvider.GetRequiredService<ISender>();
    }

    [Fact]
    public async Task HandleAsync_ShouldCreateVolunteer_WhenCommandIsValid()
    {
        //Arrange
        var command = Fixture.BuildCreateVolunteerCommand();

        // Act
        var result = await _sender.Send(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        ReadDbContext.Volunteers.Include(v => v.Pets).FirstOrDefault(v => v.Id == result.Value).Should().NotBeNull();
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnError_WhenEmailAlreadyExists()
    {
        // Arrange
        var command = Fixture.BuildCreateVolunteerCommand(email: "t@test.com");

        // Act
        var firstResult = await _sender.Send(command, CancellationToken.None);
        var secondResult = await _sender.Send(command, CancellationToken.None);

        // Assert
        firstResult.IsSuccess.Should().BeTrue();
        firstResult.Value.Should().NotBeEmpty();

        secondResult.IsSuccess.Should().BeFalse();
        secondResult.Error.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnError_WhenIdAlreadyExists()
    {
        // Arrange
        var volunteerId = VolunteerId.NewVolunteerId();
        var command = Fixture.BuildCreateVolunteerCommand(volunteerId);

        // Act
        var firstResult = await _sender.Send(command, CancellationToken.None);
        var secondResult = await _sender.Send(command, CancellationToken.None);

        // Assert
        firstResult.IsSuccess.Should().BeTrue();
        firstResult.Value.Should().NotBeEmpty();

        secondResult.IsSuccess.Should().BeFalse();
        secondResult.Error.Should().NotBeNullOrEmpty();
    }

    [Theory]
    [InlineData("", default, default, default, default, default, default, default, default, default)]
    [InlineData(default, "", default, default, default, default, default, default, default, default)]
    [InlineData(default, default, "", default, default, default, default, default, default, default)]
    [InlineData(default, default, default, default, -1, default, default, default, default, default)]
    [InlineData(default, default, default, default, default, "", default, default, default, default)]
    [InlineData(default, default, default, default, default, default, "", default, default, default)]
    [InlineData(default, default, default, default, default, default, default, "", default, default)]
    [InlineData(default, default, default, default, default, default, default, default, "", default)]
    [InlineData(default, default, default, default, default, default, default, default, default, "")]
    public async Task HandleAsync_ShouldReturnError_WhenCommandIsInvalid(
        string firstName,
        string lastName,
        string email,
        string description,
        int experienceYears,
        string phoneNumber,
        string socialNetworksName,
        string socialNetworksUrl,
        string assistanceDetailsName,
        string assistanceDetailsDescription)
    {
        var socialNetworks = new List<SocialNetworkDto> { new(socialNetworksName, socialNetworksUrl) };
        var assistanceDetails = new List<AssistanceDetailsDto>
            { new(assistanceDetailsName, assistanceDetailsDescription) };

        var command = Fixture.BuildCreateVolunteerCommand(
            VolunteerId.NewVolunteerId(),
            firstName,
            lastName,
            email,
            description,
            experienceYears,
            phoneNumber,
            socialNetworks,
            assistanceDetails);

        var result = await _sender.Send(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNullOrEmpty();
    }
}
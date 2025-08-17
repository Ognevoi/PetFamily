using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.Volunteers.DTOs;
using PetFamily.Application.Features.Volunteers.Queries.GetVolunteerById;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.TestUtils.Seeding;

namespace IntegrationTests.Volunteers;

public class GetVolunteerByIdHandlerTests : VolunteerTestBase
{
    private readonly IQueryHandler<VolunteerDto, GetVolunteerByIdQuery> _sut;

    public GetVolunteerByIdHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<IQueryHandler<VolunteerDto, GetVolunteerByIdQuery>>();
    }

    [Fact]
    public async Task GetVolunteerById_ShouldReturnVolunteer_WhenExists()
    {
        // Arrange
        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(VolunteerRepository);

        var query = new GetVolunteerByIdQuery(volunteer.Id);

        // Act
        var result = await _sut.HandleAsync(query, CancellationToken.None);

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
        var result = await _sut.HandleAsync(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();

        ReadDbContext.Volunteers.FirstOrDefault().Should().BeNull();
    }

    [Fact]
    public async Task HandleAsync_UsesCacheHit_DoesNotQueryDb()
    {
        // Arrange
        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(VolunteerRepository);
        var query = new GetVolunteerByIdQuery(volunteer.Id);

        // Act
        await _sut.HandleAsync(query, CancellationToken.None);

        // Assert
        CacheService.GetAsync<VolunteerDto>(
                CacheConstants.VOLUNTEER_PREFIX + volunteer.Id.Value, CancellationToken.None)
            .Should().NotBeNull();
    }

    [Fact]
    public async Task HandleAsync_CacheHit_DoesNotQueryDb()
    {
        // Arrange
        var volunteer = new VolunteerDto
        {
            Id = Guid.NewGuid(),
            FirstName = "Alice",
            LastName = "White",
            Email = "alice.white@test.ee",
            Description = "Experienced volunteer with a passion for animal welfare.",
            ExperienceYears = 5,
            PhoneNumber = "+3721234567",
            CreatedAt = DateTime.UtcNow
        };

        var key = CacheConstants.VOLUNTEER_PREFIX + volunteer.Id;

        await CacheService.SetAsync(
            key,
            volunteer,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CacheConstants.DEFAULT_EXPIRATION_MINUTES)
            });

        // Act
        await _sut.HandleAsync(new GetVolunteerByIdQuery(volunteer.Id), CancellationToken.None);

        // Assert
        (await CacheService.GetAsync<VolunteerDto>(key)).Should().NotBeNull();
        }

}
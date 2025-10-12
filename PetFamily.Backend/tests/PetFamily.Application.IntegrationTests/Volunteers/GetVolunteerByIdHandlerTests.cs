using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Caching;
using PetFamily.Application.Features.Volunteers.DTOs;
using PetFamily.Application.Features.Volunteers.Queries.GetVolunteerById;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;
using PetFamily.TestUtils.Seeding;

namespace IntegrationTests.Volunteers;

public class GetVolunteerByIdHandlerTests : VolunteerTestBase
{
    private readonly ISender _sender;
    private readonly ICacheService _cacheService;

    public GetVolunteerByIdHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sender = Scope.ServiceProvider.GetRequiredService<ISender>();
        _cacheService = Scope.ServiceProvider.GetRequiredService<ICacheService>();
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

    [Fact]
    public async Task CacheService_Should_SetAndGet_Successfully()
    {
        // Arrange
        var testDto = new VolunteerDto
        {
            Id = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            Description = "Test Description",
            ExperienceYears = 5,
            PhoneNumber = "+1234567890",
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };
        var key = "test_key_" + Guid.NewGuid();

        // Act - Set in cache
        await _cacheService.SetAsync(
            key,
            testDto,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            },
            CancellationToken.None);

        // Act - Get from cache
        var retrieved = await _cacheService.GetAsync<VolunteerDto>(key, CancellationToken.None);

        // Assert
        retrieved.Should().NotBeNull();
        retrieved!.Id.Should().Be(testDto.Id);
        retrieved.FirstName.Should().Be("Test");
        retrieved.Email.Should().Be("test@example.com");
    }

    [Fact]
    public async Task GetVolunteerById_Should_ReturnConsistentResults()
    {
        // Arrange
        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(VolunteerRepository);
        var query = new GetVolunteerByIdQuery(volunteer.Id);

        // Act - Call twice
        var firstResult = await _sender.Send(query, CancellationToken.None);
        var secondResult = await _sender.Send(query, CancellationToken.None);

        // Assert - Both queries should succeed and return the same data
        firstResult.IsSuccess.Should().BeTrue();
        secondResult.IsSuccess.Should().BeTrue();
        secondResult.Value.Id.Should().Be(firstResult.Value.Id);
        secondResult.Value.FirstName.Should().Be(firstResult.Value.FirstName);
        secondResult.Value.Email.Should().Be(firstResult.Value.Email);
    }

}

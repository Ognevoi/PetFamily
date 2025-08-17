using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Caching;
using PetFamily.Application.Database;
using PetFamily.Application.Features.Species;
using PetFamily.Application.Features.Volunteers;

namespace IntegrationTests.Volunteers;

public class VolunteerTestBase : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    protected readonly IntegrationTestsWebFactory Factory;
    protected readonly Fixture Fixture;
    protected readonly IServiceScope Scope;
    protected readonly IReadDbContext ReadDbContext;
    protected readonly ICacheService CacheService;
    protected readonly ISpeciesRepository SpecieRepository;
    protected readonly IVolunteersRepository VolunteerRepository;

    protected VolunteerTestBase(IntegrationTestsWebFactory factory)
    {
        Factory = factory;
        Fixture = new Fixture();
        Scope = factory.Services.CreateScope();
        ReadDbContext = Scope.ServiceProvider.GetRequiredService<IReadDbContext>();
        CacheService = Scope.ServiceProvider.GetRequiredService<ICacheService>();
        SpecieRepository = Scope.ServiceProvider.GetRequiredService<ISpeciesRepository>();
        VolunteerRepository = Scope.ServiceProvider.GetRequiredService<IVolunteersRepository>();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        Scope.Dispose();
        await Factory.ResetDataBaseAsync();
    }
}
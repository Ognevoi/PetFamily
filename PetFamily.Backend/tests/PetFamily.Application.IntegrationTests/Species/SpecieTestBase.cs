using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Database;
using PetFamily.Application.Features.Species;


namespace IntegrationTests.Species;

public class SpecieTestBase : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    private readonly IntegrationTestsWebFactory _factory;
    protected readonly Fixture Fixture;
    protected readonly IServiceScope Scope;
    protected readonly IReadDbContext ReadDbContext;
    protected readonly ISpeciesRepository SpecieRepository;


    protected SpecieTestBase(IntegrationTestsWebFactory factory)
    {
        _factory = factory;
        Fixture = new Fixture();
        Scope = factory.Services.CreateScope();
        ReadDbContext = Scope.ServiceProvider.GetRequiredService<IReadDbContext>();
        SpecieRepository = Scope.ServiceProvider.GetRequiredService<ISpeciesRepository>();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        Scope.Dispose();
        await _factory.ResetDataBaseAsync();
    }
}
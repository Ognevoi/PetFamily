using System.Data.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using PetFamily.API;
using PetFamily.Application.Database;
using PetFamily.Infrastructure.DbContexts;
using Respawn;
using Testcontainers.PostgreSql;
using Xunit;

namespace PetFamily.AcceptanceTests.Infrastructure;

public abstract class AcceptanceTestBase : IAsyncLifetime
{
    public readonly PostgreSqlContainer DbContainer;
    public WebApplicationFactory<Program> Factory = null!;
    public HttpClient Client = null!;
    public IServiceScope Scope = null!;
    public WriteDbContext WriteDbContext = null!;
    public IReadDbContext ReadDbContext = null!;
    public Respawner Respawner = null!;
    public DbConnection DbConnection = null!;

    protected AcceptanceTestBase()
    {
        DbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15")
            .WithDatabase("pet_family_test")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await DbContainer.StartAsync();
        
        // Now that the container is started, we can get the connection string
        var connectionString = DbContainer.GetConnectionString();
        
        Factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove existing database contexts
                    services.RemoveAll<WriteDbContext>();
                    services.RemoveAll<IReadDbContext>();

                    // Add test database contexts
                    services.AddScoped<WriteDbContext>(_ => new WriteDbContext(connectionString));
                    services.AddScoped<IReadDbContext>(_ => new ReadDbContext(connectionString));
                });
            });

        Client = Factory.CreateClient();
        Scope = Factory.Services.CreateScope();
        WriteDbContext = Scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        ReadDbContext = Scope.ServiceProvider.GetRequiredService<IReadDbContext>();
        DbConnection = new NpgsqlConnection(connectionString);
        
        await WriteDbContext.Database.EnsureCreatedAsync();
        await DbConnection.OpenAsync();
        
        Respawner = await Respawner.CreateAsync(DbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"],
        });
    }

    public async Task DisposeAsync()
    {
        if (Respawner != null && DbConnection != null)
        {
            await Respawner.ResetAsync(DbConnection);
        }
        
        if (DbConnection != null)
        {
            await DbConnection.CloseAsync();
            await DbConnection.DisposeAsync();
        }
        
        Scope?.Dispose();
        Client?.Dispose();
        Factory?.Dispose();
        await DbContainer.StopAsync();
        await DbContainer.DisposeAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        if (Respawner != null && DbConnection != null)
        {
            await Respawner.ResetAsync(DbConnection);
        }
    }
}

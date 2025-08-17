using System.Data.Common;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using NSubstitute;
using PetFamily.API;
using PetFamily.Application.Caching;
using PetFamily.Application.Database;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;
using PetFamily.Infrastructure.DbContexts;
using Respawn;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace IntegrationTests;

public class IntegrationTestsWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly IFilesProvider _fileProviderMock;
    private Respawner _respawner = default!;
    private DbConnection _dbConnection = default!;
    // private RedisContainer _redisContainer = default!;
    
    public IntegrationTestsWebFactory()
    {
        _fileProviderMock = Substitute.For<IFilesProvider>();
    }
    
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("pet_family")
        .WithUsername("postgres")
        .WithPassword("postgres")
        // .WithPortBinding(53860, 5432) // Used for debugging
        .Build();
    
    private readonly RedisContainer _redisContainer = new RedisBuilder()
        .WithImage("redis:latest")
        .WithPortBinding(6380, 6379) // Used for debugging
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigureDefaultServices);
    }

    protected virtual void ConfigureDefaultServices(IServiceCollection services)
    {
        // Configure PostgreSQL database
        services.RemoveAll(typeof(IFilesProvider));
        services.AddScoped(_ => _fileProviderMock);
        
        services.RemoveAll<WriteDbContext>();
        services.RemoveAll<ReadDbContext>();

        var connectionString = _dbContainer.GetConnectionString();

        services.AddScoped<WriteDbContext>(_ => new WriteDbContext(connectionString));
        services.AddScoped<IReadDbContext>(_ => new ReadDbContext(connectionString));
        
        // Configure Redis
        services.RemoveAll<ICacheService>();
        services.RemoveAll<IDistributedCache>();

        services.AddStackExchangeRedisCache(
            options =>
            {
                options.Configuration = _redisContainer.GetConnectionString();
            });

        services.AddSingleton<ICacheService, DistributedCacheService>();
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await _redisContainer.StartAsync();
        
        using var scope = Services.CreateScope();
        var writeDbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        await writeDbContext.Database.EnsureCreatedAsync();
        
        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        await InitializeRespawner();
    }
    
    private async Task InitializeRespawner()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"],
        });
    }
    
    public async Task ResetDataBaseAsync()
    {
        if (_respawner == null || _dbConnection == null)
        {
            throw new InvalidOperationException("Respawner or DbConnection is not initialized.");
        }

        await _respawner.ResetAsync(_dbConnection);
    }
    
    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
        await _redisContainer.StopAsync();
        await _redisContainer.DisposeAsync();
    }
    
    public void SetupFileProviderSuccessUploadMock()
    {
        _fileProviderMock
            .UploadFiles(Arg.Any<IEnumerable<FileData>>(), Arg.Any<string>())
            .Returns(Result.Success<IEnumerable<string>, ErrorList>(["photo1.png"]));
    }
    public void SetupFileProviderFailedUploadMock()
    {
        var errorList = new ErrorList([Errors.General.UploadFailure("test")]);
    
        _fileProviderMock
            .UploadFiles(Arg.Any<IEnumerable<FileData>>(), Arg.Any<string>())
            .Returns(Result.Failure<IEnumerable<string>, ErrorList>(errorList));
    }
    
    public void SetupFileProviderSuccessGetMock()
    {
        _fileProviderMock
            .GetFileLink(Arg.Any<IEnumerable<string>>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success<IEnumerable<string>, ErrorList>(["photo1.png"]));
    }

}
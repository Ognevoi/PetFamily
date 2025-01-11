using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.AnimalSpecies;
using PetFamily.Domain.Pets;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Infrastructure;

public class ApplicationDbContext(IConfiguration configuration) : DbContext
{
    private const string DATABASE = "DataBase";
    
    public DbSet<Pet> Pets => Set<Pet>();
    public DbSet<Volunteer> Volunteers => Set<Volunteer>();
    public DbSet<Species> Species => Set<Species>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(DATABASE));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
    
    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => builder.AddConsole());
}
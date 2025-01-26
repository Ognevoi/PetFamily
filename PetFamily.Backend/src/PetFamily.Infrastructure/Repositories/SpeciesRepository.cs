using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Domain.AnimalSpecies.Entities;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Repositories;

public class SpeciesRepository
{
    private readonly ApplicationDbContext dbContext;

    public SpeciesRepository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Guid> Add(Species species, CancellationToken cancellationToken = default)
    {
        await dbContext.Species.AddAsync(species, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return species.Id;
    }

    public async Task<Result<Species, Error>> GetById(Guid speciesId)
    {
        var species = await dbContext.Species
            .Include(s => s.Breed)
            .FirstOrDefaultAsync(s => s.Id == speciesId);

        if (species == null)
            return Errors.General.NotFound(speciesId);

        return species;
    }
}
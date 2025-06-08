using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Features.Species;
using PetFamily.Domain.Shared;
using PetFamily.Domain.SpecieManagement.AggregateRoot;
using PetFamily.Domain.SpecieManagement.Value_Objects;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Repositories;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly WriteDbContext _dbContext;

    public SpeciesRepository(WriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Add(Specie specie, CancellationToken cancellationToken = default)
    {
        await _dbContext.Species.AddAsync(specie, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return specie.Id;
    }
    
    public async Task<Guid> Save(Specie specie, CancellationToken cancellationToken = default)
    {
        _dbContext.Species.Attach(specie);    
        await _dbContext.SaveChangesAsync(cancellationToken);

        return specie.Id;
    }
    
    public async Task<Guid> Delete(Specie specie, CancellationToken cancellationToken = default)
    {
        _dbContext.Species.Remove(specie);
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return specie.Id;
    }

    public async Task<Result<Specie, Error>> GetById(SpecieId specieId, CancellationToken cancellationToken = default)
    {
        var specie = await _dbContext.Species
            .Include(s => s.Breed)
            .FirstOrDefaultAsync(s => s.Id == specieId, cancellationToken);
    
        if (specie == null)
            return Errors.General.NotFound(specieId);
    
        return specie;
    }
}
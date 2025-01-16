using Microsoft.EntityFrameworkCore;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers.Entities;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Infrastructure.Repositories;

public class VolunteersRepository
{
    private readonly ApplicationDbContext dbContext;

    public VolunteersRepository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }   

    public async Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await dbContext.Volunteers.AddAsync(volunteer, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return volunteer.Id;
    }

    public async Task<Result<Volunteer>> GetById(VolunteerId volunteerId)
    {
        var volunteer = await dbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Id == volunteerId);

        if (volunteer == null)
            return "Volunteer not found";

        return volunteer;
    }
}
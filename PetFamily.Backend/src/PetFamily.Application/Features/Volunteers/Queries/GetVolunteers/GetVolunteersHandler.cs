using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Features.Volunteers.DTOs;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Queries.GetVolunteers;

public class GetVolunteersHandler : IQueryHandler<PagedList<VolunteerDto>, GetVolunteerWithPaginationQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetVolunteersHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<PagedList<VolunteerDto>, ErrorList>> HandleAsync(GetVolunteerWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var volunteersQuery = _readDbContext.Volunteers
            .AsQueryable();
        
        volunteersQuery = volunteersQuery
            .WhereIfNotNullOrEmpty(query.Name, v => v.FirstName);
        
        return await volunteersQuery
            .OrderBy(v => v.FirstName)
            .ToPagedList(query.Page, query.Size, cancellationToken);
    }
}

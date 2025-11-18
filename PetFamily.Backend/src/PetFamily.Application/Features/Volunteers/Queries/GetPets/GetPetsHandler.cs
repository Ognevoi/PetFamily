using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Features.Volunteers.DTOs;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Queries.GetPets;

public class GetPetsHandler : IQueryHandler<GetPetWithPaginationQuery, PagedList<PetDto>>
{
    private readonly IReadDbContext _readDbContext;

    public GetPetsHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<PagedList<PetDto>, ErrorList>> Handle(GetPetWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var petsQuery = _readDbContext.Pets
            .AsQueryable();
        
        petsQuery = petsQuery
            .WhereIfNotNullOrEmpty(query.VolunteerId, p => p.VolunteerId)
            .WhereIfNotNullOrEmpty(query.Name, p => p.Name)
            .WhereIfNotNullOrEmpty(query.Age, p => DateTime.Now.Year - p.BirthDate.Year);

        petsQuery = ApplySorting(petsQuery, query.SortBy, query.SortDirection);

        return await petsQuery
            .ToPagedList(query.Page, query.Size, cancellationToken);
    }

    private static IQueryable<PetDto> ApplySorting(
        IQueryable<PetDto> query,
        string? sortBy,
        string? sortDirection)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
        {
            return query.OrderBy(p => p.Name);
        }

        var isDescending = sortDirection?.Equals("desc", StringComparison.OrdinalIgnoreCase) == true;
        var sortByLower = sortBy.ToLowerInvariant();

        return sortByLower switch
        {
            "name" => isDescending
                ? query.OrderByDescending(p => p.Name)
                : query.OrderBy(p => p.Name),
            
            "age" => isDescending
                ? query.OrderBy(p => p.BirthDate)
                : query.OrderByDescending(p => p.BirthDate),
            
            "breedid" => isDescending
                ? query.OrderByDescending(p => p.BreedId)
                : query.OrderBy(p => p.BreedId),
            
            "specieid" => isDescending
                ? query.OrderByDescending(p => p.SpecieId)
                : query.OrderBy(p => p.SpecieId),
            
            "volunteerid" => isDescending
                ? query.OrderByDescending(p => p.VolunteerId)
                : query.OrderBy(p => p.VolunteerId),
            
            _ => query.OrderBy(p => p.Name)
        };
    }
}
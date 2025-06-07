using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Features.Volunteers.DTOs;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Queries.GetPets;

public class GetPetsHandler : IQueryHandler<PagedList<PetDto>, GetPetWithPaginationQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetPetsHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<PagedList<PetDto>, ErrorList>> HandleAsync(GetPetWithPaginationQuery query,
        CancellationToken cancellationToken)
    {   
        var petsQuery = _readDbContext.Pets
            .AsQueryable();

        Expression<Func<PetDto, object>>? keySelector = query.SortBy?.ToLower() switch
        {
            "name" => p => p.Name,
            "age" => p => DateTime.Now.Year - p.BirthDate.Year,
            "breedid" => p => p.BreedId,
            "specieid" => p => p.SpecieId,
            "volunteerid" => p => p.VolunteerId,
            _ => p => p.Name
        };

        petsQuery = query.SortDirection?.ToLower() == "desc" 
            ? petsQuery.OrderByDescending(keySelector) 
            : petsQuery.OrderBy(keySelector); 
        
        petsQuery.OrderBy(keySelector);

        petsQuery = petsQuery
            .WhereIfNotNullOrEmpty(query.VolunteerId, p => p.VolunteerId)
            .WhereIfNotNullOrEmpty(query.Name, p => p.Name)
            .WhereIfNotNullOrEmpty(query.Age, p => DateTime.Now.Year - p.BirthDate.Year);

        return await petsQuery
            .ToPagedList(query.Page, query.Size, cancellationToken);
    }
}
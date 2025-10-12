using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using PetFamily.Application.Caching;
using PetFamily.Application.Database;
using PetFamily.Application.Features.Volunteers.DTOs;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Queries.GetVolunteerById;

public class GetVolunteerByIdHandler : IQueryHandler<GetVolunteerByIdQuery, VolunteerDto>
{
    private readonly IReadDbContext _readDbContext;
    private readonly ICacheService _cache;
    private readonly DistributedCacheEntryOptions _cacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CacheConstants.DEFAULT_EXPIRATION_MINUTES)
    };
    public GetVolunteerByIdHandler(IReadDbContext readDbContext, ICacheService cache)
    {
        _readDbContext = readDbContext;
        _cache = cache;
    }

    public async Task<Result<VolunteerDto, ErrorList>> Handle(GetVolunteerByIdQuery query,
        CancellationToken cancellationToken)
    {
        string key = CacheConstants.VOLUNTEER_PREFIX + query.Id;
        
        var result = await _cache.GetOrSetAsync(
            key,
            _cacheOptions,
            async () => await GetVolunteerById(query, cancellationToken),
            cancellationToken);

        if (result == null)
            return Errors.General.NotFound(query.Id).ToErrorList();

        return result;
    }

    private async Task<VolunteerDto?> GetVolunteerById(
        GetVolunteerByIdQuery query,
        CancellationToken cancellationToken)
    {
        return await _readDbContext.Volunteers
            .FirstOrDefaultAsync(p => p.Id == query.Id, cancellationToken);
    }
}

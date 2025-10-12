using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using PetFamily.Application.Caching;
using PetFamily.Application.Database;
using PetFamily.Application.Features.Volunteers.DTOs;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Queries.GetPetById;

public sealed class GetPetByIdHandler : IQueryHandler<GetPetByIdQuery, PetDto>
{
    private readonly IReadDbContext _readDbContext;
    private readonly ICacheService _cache;
    private readonly DistributedCacheEntryOptions _cacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CacheConstants.DEFAULT_EXPIRATION_MINUTES)
    };
    public GetPetByIdHandler(IReadDbContext readDbContext, ICacheService cache)
    {
        _readDbContext = readDbContext;
        _cache = cache;
    }

    public async Task<Result<PetDto, ErrorList>> Handle(GetPetByIdQuery query,
        CancellationToken cancellationToken)
    {
        string key = CacheConstants.PET_PREFIX + query.Id;

        var result = await _cache.GetOrSetAsync(
            key,
            _cacheOptions,
            async () => await GetPetById(query, cancellationToken),
            cancellationToken);

        if (result == null)
            return Errors.General.NotFound(query.Id).ToErrorList();

        return result;
    }
    
    private async Task<PetDto?> GetPetById(GetPetByIdQuery query, CancellationToken cancellationToken)
    {
        return await _readDbContext.Pets
            .FirstOrDefaultAsync(v => v.Id == query.Id, cancellationToken);
    }
}

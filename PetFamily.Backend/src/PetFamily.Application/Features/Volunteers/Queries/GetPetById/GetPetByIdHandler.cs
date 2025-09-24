using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Database;
using PetFamily.Application.Features.Volunteers.DTOs;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Queries.GetPetById;

public sealed class GetPetByIdHandler : IQueryHandler<GetPetByIdQuery, PetDto>
{
    private readonly IReadDbContext _readDbContext;

    public GetPetByIdHandler(IReadDbContext readDbContext)
        => _readDbContext = readDbContext;

    public async Task<Result<PetDto, ErrorList>> Handle(GetPetByIdQuery query,
        CancellationToken cancellationToken)
    {
        var pet = await _readDbContext.Pets
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == query.Id, cancellationToken);

        if (pet == null)
            return Errors.General.NotFound(query.Id).ToErrorList();

        return pet;
    }
}
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Database;
using PetFamily.Application.Features.Volunteers.DTOs;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Queries.GetPetById;

public class GetPetByIdHandler : IQueryHandler<PetDto, GetPetByIdQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetPetByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<PetDto, ErrorList>> HandleAsync(GetPetByIdQuery query,
        CancellationToken cancellationToken)
    {
        var pet = await _readDbContext.Pets
            .FirstOrDefaultAsync(p => p.Id == query.Id, cancellationToken);

        if (pet == null)
            return Errors.General.NotFound(query.Id).ToErrorList();

        return pet;
    }
}
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Database;
using PetFamily.Application.Features.Volunteers.DTOs;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Queries.GetVolunteerById;

public class GetVolunteerByIdHandler : IQueryHandler<VolunteerDto, GetVolunteerByIdQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetVolunteerByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<VolunteerDto, ErrorList>> HandleAsync(GetVolunteerByIdQuery query,
        CancellationToken cancellationToken)
    {
        var volunteer = await _readDbContext.Volunteers
            .FirstOrDefaultAsync(p => p.Id == query.Id, cancellationToken);

        if (volunteer == null)
            return Errors.General.NotFound(query.Id).ToErrorList();

        return volunteer;
    }
}
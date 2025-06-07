using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Volunteers.Queries.GetVolunteerById;

public record GetVolunteerByIdQuery(Guid? Id) : IQuery;
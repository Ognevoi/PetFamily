using PetFamily.Application.Features.Volunteers.DTOs;
using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Volunteers.Queries.GetPetById;

public record GetPetByIdQuery(Guid? Id) : IQuery<PetDto>;
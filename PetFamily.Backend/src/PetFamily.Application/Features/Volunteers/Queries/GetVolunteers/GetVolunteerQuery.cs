using PetFamily.Application.Features.Volunteers.DTOs;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Models;

namespace PetFamily.Application.Features.Volunteers.Queries.GetVolunteers;

public record GetVolunteerWithPaginationQuery(int Page, int Size, Guid? VolunteerId, string? Name)
    : IQuery<PagedList<VolunteerDto>>;
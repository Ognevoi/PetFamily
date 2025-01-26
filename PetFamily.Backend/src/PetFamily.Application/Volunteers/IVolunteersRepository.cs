using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers.Entities;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Application.Volunteers;

public interface IVolunteersRepository
{
    Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default);
    Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId);
    Task<Result<Volunteer, Error>> GetByEmail(Email email);
}
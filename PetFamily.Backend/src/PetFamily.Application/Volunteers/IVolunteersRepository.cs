using CSharpFunctionalExtensions;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers;

public interface IVolunteersRepository
{
    Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default);
    Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId);
    Task<Result<Volunteer, Error>> GetByEmail(Email email);
}
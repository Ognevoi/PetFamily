using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.SpecieManagement.AggregateRoot;
using PetFamily.Domain.SpecieManagement.Value_Objects;

namespace PetFamily.Application.Features.Species;

public interface ISpeciesRepository
{
    Task<Guid> Add(Specie specie, CancellationToken cancellationToken = default);
    Task<Guid> Save(Specie specie, CancellationToken cancellationToken = default);
    Task<Guid> Delete(Specie specie, CancellationToken cancellationToken = default);
    Task<Result<Specie, Error>> GetById(SpecieId specieId, CancellationToken cancellationToken = default);
    
}

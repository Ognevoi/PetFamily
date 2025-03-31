using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.SpecieManagement.Value_Objects;
using PetFamily.Domain.SpecieManagement.AggregateRoot;
namespace PetFamily.Application.Species;

public interface ISpeciesRepository
{
    Task<Guid> Add(Specie specie, CancellationToken cancellationToken = default);
    Task<Guid> Save(Specie specie, CancellationToken cancellationToken = default);
    Task<Guid> Delete(Specie specie, CancellationToken cancellationToken = default);
    Task<Result<Specie, Error>> GetById(SpecieId specieId, CancellationToken cancellationToken = default);
    
}

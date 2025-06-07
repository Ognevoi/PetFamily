using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Interfaces;

public interface IQueryHandler<TResponse, in TQuery> where TQuery : IQuery
{
    public Task<Result<TResponse, ErrorList>> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}

public interface IQueryHandler<in TQuery> where TQuery : IQuery
{
    public Task<UnitResult<ErrorList>> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
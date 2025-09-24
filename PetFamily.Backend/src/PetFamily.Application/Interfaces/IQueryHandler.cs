using CSharpFunctionalExtensions;
using MediatR;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Interfaces;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse, ErrorList>>
    where TQuery : IQuery<TResponse>;
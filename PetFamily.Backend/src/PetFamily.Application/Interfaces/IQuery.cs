using CSharpFunctionalExtensions;
using MediatR;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Interfaces;

public interface IQuery<TResponse> : IRequest<Result<TResponse, ErrorList>>;
using CSharpFunctionalExtensions;
using MediatR;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Interfaces;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse, ErrorList>>
    where TCommand : ICommand<TResponse>;
using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using PetFamily.Application.Extensions;

namespace PetFamily.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationFailures = await Task.WhenAll(
            _validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

        var failures = validationFailures
            .Where(result => !result.IsValid)
            .ToList();

        if (failures.Count != 0)
        {
            var failure = failures.ToErrorList();

            var successType = typeof(TResponse).GetGenericArguments()[0];
            var errorType = typeof(TResponse).GetGenericArguments()[1];

            var toErrorListMethod = typeof(Result)
                .GetMethods()
                .First(t => t.Name == "Failure" &&
                            t.GetParameters().Length == 1 &&
                            t.GetGenericArguments().Length == 2)
                .MakeGenericMethod(successType, errorType);

            return (TResponse)toErrorListMethod.Invoke(null, [failure])!;
        }

        var response = await next();

        return response;
    }
}
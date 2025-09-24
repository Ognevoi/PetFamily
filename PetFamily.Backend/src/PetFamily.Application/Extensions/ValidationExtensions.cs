using FluentValidation.Results;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Extensions;

public static class ValidationExtensions
{
    public static ErrorList ToErrorList(this ValidationResult validationResult) =>
        validationResult.Errors.Select(e => Error.Deserialize(e.ErrorMessage)).ToList();

    public static ErrorList ToErrorList(this IEnumerable<ValidationResult> validationResult) =>
        validationResult.SelectMany(e => e.Errors).Select(e => Error.Deserialize(e.ErrorMessage)).ToList();
}
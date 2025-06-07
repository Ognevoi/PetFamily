using FluentValidation.Results;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Extensions;

public static class ValidationExtensions
{
    public static ErrorList ToErrorList(this ValidationResult validationResult)
    {
        return validationResult.Errors
            .Select(e => Error.Deserialize(e.ErrorMessage))
            .ToList();
    }
}
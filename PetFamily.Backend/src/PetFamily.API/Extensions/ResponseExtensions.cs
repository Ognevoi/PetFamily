using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Response;
using PetFamily.Domain.Shared;
using FluentValidation.Results;


namespace PetFamily.API.Extensions;

public static class ResponseExtensions
{
    public static ActionResult ToResponse(this Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
        
        var responseError = new ResponseError(error.Code, error.Message, null);
    
        var envelope = Envelope.Error([responseError]);
    
        return new ObjectResult(envelope)
        {
            StatusCode = statusCode
        };
    }
    
    public static ActionResult ToResponse(this ErrorList errorList)
     {
         if (!errorList.Any())
         {
             return new ObjectResult(null)
             {
                 StatusCode = StatusCodes.Status500InternalServerError
             };
         }

         var distinctErrorTypes = errorList
             .Select(e => e.Type)
             .Distinct()
             .ToList();

         var statusCode = distinctErrorTypes.Count > 1
             ? StatusCodes.Status500InternalServerError :
             GetStatusCodeForErrorType(errorList.First().Type);
         
         
         var responseErrors = errorList.Select(error => new ResponseError(error.Code, error.Message, null)).ToList();
         
         var envelope = Envelope.Error(responseErrors);

         return new ObjectResult(envelope) { StatusCode = statusCode };
     }

    
    public static ActionResult ToValidationErrorResponse(this ValidationResult result)
    {
        if (result.IsValid)
            throw new InvalidOperationException("Validation result is valid");

        var validationErrors = result.Errors;

        var responseErrors = from validationError in validationErrors
            let errorMessage = validationError.ErrorMessage
            let error = Error.Deserialize(errorMessage)
            select new ResponseError(error.Code, error.Message, validationError.PropertyName);
        
        var envelope = Envelope.Error(responseErrors);
        
        return new ObjectResult(envelope)
        {
            StatusCode = StatusCodes.Status400BadRequest
        };

    }
    private static int GetStatusCodeForErrorType(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };    
}
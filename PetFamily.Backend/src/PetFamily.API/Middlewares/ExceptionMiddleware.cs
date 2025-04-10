using System.Text.Json;
using PetFamily.API.Response;

namespace PetFamily.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);

            ResponseError responseError;

            if (e is JsonException)
            {
                responseError = new ResponseError(
                    "validation.invalidFormat",
                    "Invalid input format. Possibly incorrect type in one of the fields.",
                    TryExtractFieldFromJsonException(e.Message)
                );

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            else
            {
                responseError = new ResponseError("server.internal", e.Message, null);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            var envelope = Envelope.Error([responseError]);
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(envelope);
        }
    }
    
    private string? TryExtractFieldFromJsonException(string message)
    {
        var match = System.Text.RegularExpressions.Regex.Match(message, @"Path: \$\.(\w+)");
        return match.Success ? match.Groups[1].Value : null;
    }
    
}


public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}
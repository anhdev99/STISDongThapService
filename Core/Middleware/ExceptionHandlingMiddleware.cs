using Core.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Core.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occured: {Message}", ex.Message);

            if (ex is ValidationException validationException)
                _logger.LogError(validationException, "Exception occurred: {Message} {@Errors} {@Exception}",
                    ex.Message, validationException.Errors, validationException);
            else
                _logger.LogError(ex, "Exception occured: {Message}", ex.Message);

            var exceptionDetails = ExceptionHelper.GetExceptionDetail(ex);

            context.Response.StatusCode = exceptionDetails.Code;

            await context.Response.WriteAsJsonAsync(exceptionDetails);
        }
    }
}
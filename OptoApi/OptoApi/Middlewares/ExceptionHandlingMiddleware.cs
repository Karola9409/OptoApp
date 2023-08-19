using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace OptoApi.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, $"An unhandled exception has occurred, {ex.Message}");

        var problemDetails = new ProblemDetails
        {
            Title = "Internal Server Error",
            Status = (int)HttpStatusCode.InternalServerError,
            Instance = context.Request.Path,
            Detail = "Internal server error occured!"
        };

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        var result = JsonSerializer.Serialize(problemDetails);

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(result);
    }
}
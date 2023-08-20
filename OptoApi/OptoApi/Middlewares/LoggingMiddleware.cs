using System.Diagnostics;

namespace OptoApi.Middlewares;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task Invoke(HttpContext context)
    {
        var stopwatch = new Stopwatch();
        _logger.LogInformation($"Started processing request {context.Request.Path}");
        stopwatch.Start();
        
        await _next(context);
        
        stopwatch.Stop();
        _logger.LogInformation($"Finished processing request {context.Request.Path} in {stopwatch.ElapsedMilliseconds} ms");
    }
    
}
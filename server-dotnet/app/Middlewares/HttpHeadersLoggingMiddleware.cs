namespace server_dotnet.Middlewares;

public class HttpHeadersLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<HttpHeadersLoggingMiddleware> _logger;

    public HttpHeadersLoggingMiddleware(RequestDelegate next, ILogger<HttpHeadersLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            foreach (var header in context.Request.Headers)
            {
                _logger.LogDebug("Header: {Key} = {Value}", header.Key, header.Value);
            }
        }

        await _next(context);
    }
}

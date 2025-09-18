using System.Diagnostics;

namespace UserManagementAPI.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var method = context.Request.Method;
            var path = context.Request.Path;

            await _next(context);

            stopwatch.Stop();
            var statusCode = context.Response.StatusCode;

            _logger.LogInformation("HTTP {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds}ms",
                method, path, statusCode, stopwatch.ElapsedMilliseconds);
        }
    }
}
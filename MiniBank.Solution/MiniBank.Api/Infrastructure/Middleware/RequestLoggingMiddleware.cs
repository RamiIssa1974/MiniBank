
using System.Diagnostics;

namespace MiniBank.Api.Infrastructure.Middleware
{
    public class RequestLoggingMiddleware : IMiddleware
    {
        private const string CorrelationHeader = "X-Correlation-Id";
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Correlation
            if (!context.Request.Headers.TryGetValue(CorrelationHeader, out var correlationId) || string.IsNullOrWhiteSpace(correlationId))
            {
                correlationId = Guid.NewGuid().ToString("N");
                context.Request.Headers[CorrelationHeader] = correlationId;
                context.Request.Headers["Rami-Issa-header"] = "Mini Bank Application August 2025";
            }
            context.Response.Headers[CorrelationHeader] = correlationId!;
            context.Response.Headers["Rami-Issa-header"] = "Mini Bank Application August 2025";
            var sw = Stopwatch.StartNew();

            _logger.LogInformation("[Rami Issa] REQ {Method} {Path} Correlation={CorrelationId}",
                context.Request.Method, context.Request.Path, correlationId);

            await next(context);

            sw.Stop();
            context.Response.Headers["X-Elapsed-Ms"] = sw.ElapsedMilliseconds.ToString();

            _logger.LogInformation("[Rami Issa] RES {StatusCode} {Method} {Path} {Elapsed}ms Correlation={CorrelationId}",
                context.Response.StatusCode, context.Request.Method, context.Request.Path, sw.ElapsedMilliseconds, correlationId);
        }
    }
}

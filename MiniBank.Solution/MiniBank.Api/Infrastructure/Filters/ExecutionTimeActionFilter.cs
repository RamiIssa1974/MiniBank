// File: Infrastructure/Filters/ExecutionTimeActionFilter.cs
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace MiniBank.Api.Infrastructure.Filters;

public class ExecutionTimeActionFilter : IAsyncActionFilter
{
    private const string CorrelationHeader = "X-Correlation-Id";
    private readonly ILogger<ExecutionTimeActionFilter> _logger;

    public ExecutionTimeActionFilter(ILogger<ExecutionTimeActionFilter> logger) => _logger = logger;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var sw = Stopwatch.StartNew();
        var executed = await next();
        sw.Stop();

        var corr = context.HttpContext.Request.Headers.TryGetValue(CorrelationHeader, out StringValues v)
            ? v.ToString() : context.HttpContext.TraceIdentifier;

        context.HttpContext.Response.Headers["X-Action-Duration-ms"] = sw.ElapsedMilliseconds.ToString();

        var ctrl = context.Controller.GetType().Name;
        var action = context.ActionDescriptor.DisplayName;
        var status = executed?.Exception == null ? "OK" : "ERROR";
        _logger.LogInformation("ACTION {Controller} {Action} -> {Status} in {Elapsed}ms Corr={Correlation}",
            ctrl, action, status, sw.ElapsedMilliseconds, corr);
    }
}

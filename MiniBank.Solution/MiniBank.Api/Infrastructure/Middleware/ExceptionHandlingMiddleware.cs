
using System.Net;
using System.Text.Json;

namespace MiniBank.Api.Infrastructure.Middleware
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var traceId = context.TraceIdentifier;
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var payload = new
                {
                    error = "[Rami Issa] Unexpected error occurred.",
                    traceId,
                    #if DEBUG
                        details = ex.Message
                    #endif
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
            }
        }
    }
}

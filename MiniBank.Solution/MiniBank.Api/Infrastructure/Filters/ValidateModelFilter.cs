// File: Infrastructure/Filters/ValidateModelFilter.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MiniBank.Api.Infrastructure.Filters;

public class ValidateModelFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(kv => kv.Value?.Errors.Count > 0)
                .ToDictionary(
                    kv => kv.Key,
                    kv => kv.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            var payload = new ValidationErrorResponse
            {
                Message = "Validation failed.",
                Errors = errors
            };

            context.Result = new BadRequestObjectResult(payload);
        }
    }
   
    public void OnActionExecuted(ActionExecutedContext context) { }
}
public sealed class ValidationErrorResponse
{
    public string Message { get; set; } = default!;
    public Dictionary<string, string[]> Errors { get; set; } = new();
}


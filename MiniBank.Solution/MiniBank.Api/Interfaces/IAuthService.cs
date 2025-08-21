using System.Security.Claims;

namespace MiniBank.Api.Interfaces;

public interface IAuthService
{
    /// מחזיר ClaimsPrincipal אם האישורים תקינים; אחרת null
    Task<ClaimsPrincipal?> ValidateAsync(string username, string password, CancellationToken ct = default);
}

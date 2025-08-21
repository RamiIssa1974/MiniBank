using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MiniBank.Api.Data;
using MiniBank.Api.Interfaces;

namespace MiniBank.Api.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    public AuthService(AppDbContext db) => _db = db;

    public async Task<ClaimsPrincipal?> ValidateAsync(string username, string password, CancellationToken ct = default)
    {
        var user = await _db.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserName == username, ct);
        if (user is null) return null;

        var ok = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        if (!ok) return null;

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
        };
        if (user.CustomerId.HasValue)
            claims.Add(new("customerId", user.CustomerId.Value.ToString()));

        var identity = new ClaimsIdentity(claims, "Basic");
        return new ClaimsPrincipal(identity);
    }
}

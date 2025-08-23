using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using MiniBank.Api.Data;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly AppDbContext _db;

    public BasicAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        AppDbContext db) : base(options, logger, encoder, clock)
    {
        _db = db;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return AuthenticateResult.Fail("Missing Authorization Header");

        try
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            var credentialBytes = Convert.FromBase64String(authHeader.Replace("Basic ", "", StringComparison.OrdinalIgnoreCase));
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
            var username = credentials[0];
            var password = credentials[1];

            var user = _db.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return AuthenticateResult.Fail("Invalid Username or Password");
            }

            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User"),
                new ("userId", user.Id.ToString())
            };

            if (user.CustomerId.HasValue)
            {
                claims.Add(new Claim("customerId", user.CustomerId.Value.ToString()));
            }
            var identity = new ClaimsIdentity(claims,
                                                Scheme.Name,
                                                ClaimTypes.Name,
                                                ClaimTypes.Role);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        catch
        {
            return AuthenticateResult.Fail("Invalid Authorization Header");
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MiniBank.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // require any authenticated user (use Roles="Admin" if you want admin-only)
    [HttpGet("me")]
    [Authorize(Roles = "Admin,User")]
    public IActionResult Me()
    {
        var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();
        var res = new
        {
            user = User.Identity?.Name,
            roles,
            customerId = User.FindFirst("CustomerId")?.Value
        };
        return Ok(res);
    }
}

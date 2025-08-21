// Controllers/AuthController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniBank.Api.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            user = User.Identity?.Name,
            role = User.IsInRole("Admin") ? "Admin" : "User",
            customerId = User.FindFirst("customerId")?.Value
        });
    }
}

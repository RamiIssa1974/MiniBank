using Microsoft.AspNetCore.Mvc;

namespace MiniBank.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() =>
        Ok(new { status = "OK", timeUtc = DateTime.UtcNow });
}

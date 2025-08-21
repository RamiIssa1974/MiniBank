using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBank.Api.Data;

namespace MiniBank.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class HealthController : ControllerBase
{
    private readonly AppDbContext _db;
    public HealthController(AppDbContext db) => _db = db;

    [HttpGet]
    public IActionResult Get() =>
        Ok(new { status = "OK", timeUtc = DateTime.UtcNow });

    [HttpGet("db")]
    public async Task<IActionResult> Db()
    {
        var canConnect = await _db.Database.CanConnectAsync();
        var customers = await _db.Customers.CountAsync();
        var accounts = await _db.Accounts.CountAsync();
        return Ok(new { canConnect, customers, accounts, timeUtc = DateTime.UtcNow });
    }
}

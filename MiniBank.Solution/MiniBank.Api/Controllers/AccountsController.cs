using Microsoft.AspNetCore.Mvc;
using MiniBank.Api.Interfaces;

namespace MiniBank.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountRepository _repo;
    public AccountsController(IAccountRepository repo) => _repo = repo;

    // GET: /api/accounts
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var items = await _repo.GetAllAsync(ct);
        return Ok(items);
    }

    // GET: /api/accounts/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var item = await _repo.GetByIdAsync(id, ct);
        if (item is null) return NotFound();
        return Ok(item);
    }

    // GET: /api/accounts/by-customer/3
    [HttpGet("by-customer/{customerId:int}")]
    public async Task<IActionResult> GetByCustomer(int customerId, CancellationToken ct)
    {
        var items = await _repo.GetByCustomerAsync(customerId, ct);
        return Ok(items);
    }
}

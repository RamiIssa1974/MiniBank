using Microsoft.AspNetCore.Mvc;
using MiniBank.Api.Interfaces;

namespace MiniBank.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _repo;
    public CustomersController(ICustomerRepository repo) => _repo = repo;

    // GET: /api/customers
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var items = await _repo.GetAllAsync(ct);
        return Ok(items);
    }

    // GET: /api/customers/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var item = await _repo.GetByIdAsync(id, ct);
        if (item is null) return NotFound();
        return Ok(item);
    }
}

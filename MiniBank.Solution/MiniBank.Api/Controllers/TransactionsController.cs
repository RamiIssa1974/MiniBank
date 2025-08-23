using Microsoft.AspNetCore.Mvc;
using MiniBank.Api.Dtos;
using MiniBank.Api.Infrastructure.Filters;
using MiniBank.Api.Interfaces;

namespace MiniBank.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionRepository _repo;
    private readonly ITransactionService _svc;

    public TransactionsController(ITransactionRepository repo, ITransactionService svc)
    {
        _repo = repo;
        _svc = svc;
    }

    // GET: /api/transactions/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var item = await _repo.GetByIdAsync(id, ct);
        if (item is null) return NotFound();
        return Ok(item);
    }

    // GET: /api/transactions
    // דוגמה: /api/transactions?accountId=1&type=Deposit&dateFrom=2025-01-01&dateTo=2025-12-31&sort=desc&page=1&pageSize=20
    [HttpGet]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int? accountId,
        [FromQuery] string? type,
        [FromQuery] DateTime? dateFrom,
        [FromQuery] DateTime? dateTo,
        [FromQuery] string? sort = "desc",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var sortDesc = string.Equals(sort, "desc", StringComparison.OrdinalIgnoreCase);
        var result = await _repo.GetPagedAsync(accountId, type, dateFrom, dateTo, sortDesc, page, pageSize, ct);
        return Ok(result);
    }

    [ServiceFilter(typeof(ExecutionTimeActionFilter))]
    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit([FromBody] DepositWithdrawRequest req, CancellationToken ct)
    {
        try
        {
            var result = await _svc.DepositAsync(req, ct);
            return CreatedAtAction(nameof(GetById), new { id = result.TransactionId }, result);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { error = ex.Message }); }
    }

    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw([FromBody] DepositWithdrawRequest req, CancellationToken ct)
    {
        try
        {
            var result = await _svc.WithdrawAsync(req, ct);
            return CreatedAtAction(nameof(GetById), new { id = result.TransactionId }, result);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { error = ex.Message }); }
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer([FromBody] TransferRequest req, CancellationToken ct)
    {
        try
        {
            var (fromSide, toSide) = await _svc.TransferAsync(req, ct);
            return Ok(new { fromSide, toSide });
        }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { error = ex.Message }); }
    }
}

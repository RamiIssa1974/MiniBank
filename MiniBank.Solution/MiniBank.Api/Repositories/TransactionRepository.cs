using Microsoft.EntityFrameworkCore;
using MiniBank.Api.Data;
using MiniBank.Api.Interfaces;
using MiniBank.Api.Models;

namespace MiniBank.Api.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _db;
    public TransactionRepository(AppDbContext db) => _db = db;

    public Task<TransactionEntry?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _db.Transactions
           .AsNoTracking()
           .FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task<PagedResult<TransactionEntry>> GetPagedAsync(
        int? accountId = null,
        string? type = null,
        DateTime? dateFrom = null,
        DateTime? dateTo = null,
        bool sortDesc = true,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;

        var q = _db.Transactions.AsNoTracking().AsQueryable();

        if (accountId is not null)
            q = q.Where(t => t.AccountId == accountId);

        if (!string.IsNullOrWhiteSpace(type))
        {
            if (Enum.TryParse<TransactionType>(type, true, out var parsedType))
            {
                q = q.Where(t => t.Type == parsedType);
            }
        }


        if (dateFrom is not null)
            q = q.Where(t => t.CreatedAt >= dateFrom);

        if (dateTo is not null)
            q = q.Where(t => t.CreatedAt <= dateTo);

        q = sortDesc
            ? q.OrderByDescending(t => t.CreatedAt).ThenByDescending(t => t.Id)
            : q.OrderBy(t => t.CreatedAt).ThenBy(t => t.Id);

        var total = await q.CountAsync(ct);

        var items = await q
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<TransactionEntry>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }
}

using Microsoft.EntityFrameworkCore;
using MiniBank.Api.Data;
using MiniBank.Api.Interfaces;
using MiniBank.Api.Models;

namespace MiniBank.Api.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _db;
    public AccountRepository(AppDbContext db) => _db = db;

    public Task<Account?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _db.Accounts
           .AsNoTracking()
           .Include(a => a.Customer)
           .FirstOrDefaultAsync(a => a.Id == id, ct);

    public Task<List<Account>> GetByCustomerAsync(int customerId, CancellationToken ct = default) =>
        _db.Accounts
           .AsNoTracking()
           .Where(a => a.CustomerId == customerId)
           .OrderBy(a => a.Id)
           .ToListAsync(ct);

    public Task<List<Account>> GetAllAsync(CancellationToken ct = default) =>
        _db.Accounts
           .AsNoTracking()
           .OrderBy(a => a.Id)
           .ToListAsync(ct);
}

using Microsoft.EntityFrameworkCore;
using MiniBank.Api.Data;
using MiniBank.Api.Interfaces;
using MiniBank.Api.Models;

namespace MiniBank.Api.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _db;
    public CustomerRepository(AppDbContext db) => _db = db;

    public Task<List<Customer>> GetAllAsync(CancellationToken ct = default)
        => _db.Customers
              .AsNoTracking()
              .OrderBy(c => c.Id)
              .ToListAsync(ct);

    public Task<Customer?> GetByIdAsync(int id, CancellationToken ct = default)
        => _db.Customers
              .AsNoTracking()
              .FirstOrDefaultAsync(c => c.Id == id, ct);
}

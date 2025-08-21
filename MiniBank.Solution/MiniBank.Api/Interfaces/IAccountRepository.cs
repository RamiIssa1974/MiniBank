using MiniBank.Api.Models;

namespace MiniBank.Api.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<List<Account>> GetByCustomerAsync(int customerId, CancellationToken ct = default);
    Task<List<Account>> GetAllAsync(CancellationToken ct = default);
}

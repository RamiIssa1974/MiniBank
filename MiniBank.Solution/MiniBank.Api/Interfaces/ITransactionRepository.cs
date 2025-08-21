using MiniBank.Api.Models;

namespace MiniBank.Api.Interfaces;

public interface ITransactionRepository
{
    Task<TransactionEntry?> GetByIdAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// שליפה עם סינון אופציונלי + מיון + פאג'ינציה
    /// </summary>
    Task<PagedResult<TransactionEntry>> GetPagedAsync(
        int? accountId = null,
        string? type = null,
        DateTime? dateFrom = null,
        DateTime? dateTo = null,
        bool sortDesc = true,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default);
}

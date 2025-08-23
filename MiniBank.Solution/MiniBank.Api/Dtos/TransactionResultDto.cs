using MiniBank.Api.Models;

namespace MiniBank.Api.Dtos;

public class TransactionResultDto
{
    public int TransactionId { get; set; }
    public int AccountId { get; set; }
    public decimal NewBalance { get; set; }
    public string Type { get; set; } = string.Empty; // Deposit/Withdraw/TransferIn/Out
    public DateTime CreatedAt { get; set; }

    public static TransactionResultDto From(Account a, TransactionEntry t) => new()
    {
        TransactionId = t.Id,
        AccountId = a.Id,
        NewBalance = a.Balance,
        Type = t.Type.ToString(),
        CreatedAt = t.CreatedAt
    };
}

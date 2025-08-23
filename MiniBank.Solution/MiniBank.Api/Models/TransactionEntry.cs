using System.ComponentModel.DataAnnotations.Schema;

namespace MiniBank.Api.Models;

public class TransactionEntry
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public Account? Account { get; set; }

    public TransactionType Type { get; set; } = TransactionType.Deposit; // Deposit/Withdraw/TransferIn/TransferOut

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // לקישור בין TransferIn/TransferOut (לא חובה בשלב הזה)
    public Guid? ReferenceId { get; set; }
}

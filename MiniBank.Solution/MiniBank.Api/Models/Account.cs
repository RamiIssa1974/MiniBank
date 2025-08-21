using System.ComponentModel.DataAnnotations.Schema;

namespace MiniBank.Api.Models;

public class Account
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public string Number { get; set; } = string.Empty; // מספר חשבון
    public string Currency { get; set; } = "ILS";

    [Column(TypeName = "decimal(18,2)")]
    public decimal Balance { get; set; } = 0m;

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

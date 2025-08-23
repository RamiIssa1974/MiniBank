using System.ComponentModel.DataAnnotations;

namespace MiniBank.Api.Dtos;

public class DepositWithdrawRequest
{
    public int AccountId { get; set; }
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Amount must be >= 0")]
    public decimal Amount { get; set; } // > 0
    public string? Description { get; set; }
}

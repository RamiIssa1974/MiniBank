namespace MiniBank.Api.Dtos;

public class DepositWithdrawRequest
{
    public int AccountId { get; set; }
    public decimal Amount { get; set; } // > 0
    public string? Description { get; set; }
}

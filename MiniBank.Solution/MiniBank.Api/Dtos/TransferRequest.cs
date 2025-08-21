namespace MiniBank.Api.Dtos;

public class TransferRequest
{
    public int FromAccountId { get; set; }
    public int ToAccountId { get; set; }
    public decimal Amount { get; set; } // > 0
    public string? Description { get; set; }
}

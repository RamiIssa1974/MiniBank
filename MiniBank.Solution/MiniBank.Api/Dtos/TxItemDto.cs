namespace MiniBank.Api.Dtos
{
    public record TxItemDto(long Id, int AccountId, string Type, decimal Amount, DateTime CreatedAt);

}

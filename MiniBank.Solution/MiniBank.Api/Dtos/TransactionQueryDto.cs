namespace MiniBank.Api.Dtos
{
    public record TransactionQueryDto(
    int? AccountId, int Page = 1, int PageSize = 20,
    string? Type = null, DateTime? From = null, DateTime? To = null);

}

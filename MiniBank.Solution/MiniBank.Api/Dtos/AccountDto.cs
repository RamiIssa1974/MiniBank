namespace MiniBank.Api.Dtos
{
    public record AccountDto(int Id, string Iban, decimal Balance, bool IsLocked);

}

using MiniBank.Api.Dtos;

namespace MiniBank.Api.Interfaces;

public interface ITransactionService
{
    Task<TransactionResultDto> DepositAsync(DepositWithdrawRequest req, CancellationToken ct = default);
    Task<TransactionResultDto> WithdrawAsync(DepositWithdrawRequest req, CancellationToken ct = default);
    Task<(TransactionResultDto fromSide, TransactionResultDto toSide)> TransferAsync(TransferRequest req, CancellationToken ct = default);
}

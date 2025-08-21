using Microsoft.EntityFrameworkCore;
using MiniBank.Api.Data;
using MiniBank.Api.Dtos;
using MiniBank.Api.Interfaces;
using MiniBank.Api.Models;

namespace MiniBank.Api.Services;

public class TransactionService : ITransactionService
{
    private readonly AppDbContext _db;
    public TransactionService(AppDbContext db) => _db = db;

    public async Task<TransactionResultDto> DepositAsync(DepositWithdrawRequest req, CancellationToken ct = default)
    {
        if (req.Amount <= 0m) throw new ArgumentException("Amount must be greater than zero.");
        var account = await _db.Accounts.FirstOrDefaultAsync(a => a.Id == req.AccountId && a.IsActive, ct)
                      ?? throw new KeyNotFoundException("Account not found or inactive.");

        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        account.Balance += req.Amount;

        var entry = new TransactionEntry
        {
            AccountId = account.Id,
            Type = "Deposit",
            Amount = req.Amount,
            Description = req.Description,
            CreatedAt = DateTime.UtcNow
        };
        _db.Transactions.Add(entry);
        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);

        return TransactionResultDto.From(account, entry);
    }

    public async Task<TransactionResultDto> WithdrawAsync(DepositWithdrawRequest req, CancellationToken ct = default)
    {
        if (req.Amount <= 0m) throw new ArgumentException("Amount must be greater than zero.");
        var account = await _db.Accounts.FirstOrDefaultAsync(a => a.Id == req.AccountId && a.IsActive, ct)
                      ?? throw new KeyNotFoundException("Account not found or inactive.");

        if (account.Balance < req.Amount)
            throw new InvalidOperationException("Insufficient funds.");

        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        account.Balance -= req.Amount;

        var entry = new TransactionEntry
        {
            AccountId = account.Id,
            Type = "Withdraw",
            Amount = req.Amount,
            Description = req.Description,
            CreatedAt = DateTime.UtcNow
        };
        _db.Transactions.Add(entry);
        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);

        return TransactionResultDto.From(account, entry);
    }

    public async Task<(TransactionResultDto fromSide, TransactionResultDto toSide)> TransferAsync(TransferRequest req, CancellationToken ct = default)
    {
        if (req.Amount <= 0m) throw new ArgumentException("Amount must be greater than zero.");
        if (req.FromAccountId == req.ToAccountId) throw new ArgumentException("From and To accounts must be different.");

        var from = await _db.Accounts.FirstOrDefaultAsync(a => a.Id == req.FromAccountId && a.IsActive, ct)
                   ?? throw new KeyNotFoundException("Source account not found or inactive.");

        var to = await _db.Accounts.FirstOrDefaultAsync(a => a.Id == req.ToAccountId && a.IsActive, ct)
                 ?? throw new KeyNotFoundException("Destination account not found or inactive.");

        if (from.Currency != to.Currency)
            throw new InvalidOperationException("Currency mismatch between accounts.");

        if (from.Balance < req.Amount)
            throw new InvalidOperationException("Insufficient funds.");

        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        // עדכון יתרות
        from.Balance -= req.Amount;
        to.Balance += req.Amount;

        // שני רישומי תנועה עם אותו ReferenceId
        var reference = Guid.NewGuid();

        var outEntry = new TransactionEntry
        {
            AccountId = from.Id,
            Type = "TransferOut",
            Amount = req.Amount,
            Description = req.Description,
            CreatedAt = DateTime.UtcNow,
            ReferenceId = reference
        };
        var inEntry = new TransactionEntry
        {
            AccountId = to.Id,
            Type = "TransferIn",
            Amount = req.Amount,
            Description = req.Description,
            CreatedAt = DateTime.UtcNow,
            ReferenceId = reference
        };

        _db.Transactions.AddRange(outEntry, inEntry);
        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);

        return (TransactionResultDto.From(from, outEntry), TransactionResultDto.From(to, inEntry));
    }
}

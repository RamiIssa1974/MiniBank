using Microsoft.EntityFrameworkCore;
using MiniBank.Api.Models;

namespace MiniBank.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<TransactionEntry> Transactions => Set<TransactionEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // אינדקסים שימושיים
        modelBuilder.Entity<Account>()
            .HasIndex(a => a.Number)
            .IsUnique(false);

        modelBuilder.Entity<TransactionEntry>()
            .HasIndex(t => new { t.AccountId, t.CreatedAt });

        // קשרים
        modelBuilder.Entity<Account>()
            .HasOne(a => a.Customer)
            .WithMany(c => c.Accounts)
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

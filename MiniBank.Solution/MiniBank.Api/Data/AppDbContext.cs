using Microsoft.EntityFrameworkCore;
using MiniBank.Api.Models;

namespace MiniBank.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<TransactionEntry> Transactions => Set<TransactionEntry>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // אינדקסים שימושיים
        modelBuilder.Entity<Account>()
            .HasIndex(a => a.Number)
            .IsUnique(false);

        modelBuilder.Entity<TransactionEntry>()
            .Property(t => t.Type)
            .HasConversion<string>();

        modelBuilder.Entity<TransactionEntry>()
            .HasIndex(t => new { t.AccountId, t.CreatedAt });

        // קשרים
        modelBuilder.Entity<Account>()
            .HasOne(a => a.Customer)
            .WithMany(c => c.Accounts)
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<User>()
            .HasIndex(u => u.UserName)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasOne(u => u.Customer)
            .WithMany() // אין אוסף Users ב-Customer כרגע
            .HasForeignKey(u => u.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

using System.Security.Principal;

namespace MiniBank.Api.Models;

public class Customer
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}

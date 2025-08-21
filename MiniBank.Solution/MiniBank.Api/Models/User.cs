namespace MiniBank.Api.Models;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty; // ייחודי
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "User"; // "Admin" / "User"
    public int? CustomerId { get; set; } // למשתמשי קצה הקשורים ללקוח
    public Customer? Customer { get; set; }

    public bool IsActive { get; set; } = true;
    public bool IsAdmin { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

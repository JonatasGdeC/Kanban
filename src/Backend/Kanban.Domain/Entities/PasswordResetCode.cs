namespace Kanban.Domain.Entities;

public class PasswordResetCode
{
    public Guid UserId { get; set; }
    public required string CodeHash { get; set; }
    public int Attempts { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime ExpiresAt { get; set; }
    
    public bool IsValid => Attempts < 3 && ExpiresAt > DateTime.UtcNow;
}
namespace WorkTree.API.Entities;

public class RefreshToken : EntityBase
{
    public string HashToken { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsRevoked => RevokedAt.HasValue;
    public bool IsActive => !IsExpired && !IsRevoked;


    public void Touch()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
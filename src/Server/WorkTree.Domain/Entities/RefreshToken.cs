namespace WorkTree.Domain.Entities;

public class RefreshToken : EntityBase
{
    public string TokenHash { get; private set; } = string.Empty;
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public DateTime ExpiresAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsRevoked => RevokedAt.HasValue;
    public bool IsActive => !IsExpired && !IsRevoked;


    public RefreshToken()
    {
    }

    public RefreshToken(string tokenHash, Guid userId, DateTime expiresAt)
    {
        TokenHash = tokenHash;
        UserId = userId;
        ExpiresAt = expiresAt;
    }

    public void Revoke()
    {
        if (IsRevoked)
            return;

        RevokedAt = DateTime.UtcNow;
        Touch();
    }

    private void Touch()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
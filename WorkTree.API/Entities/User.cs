namespace WorkTree.API.Entities;

public class User : EntityBase
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public Guid TenantId { get; private set; }

    private User()
    {
    }

    public User(string name, string email, Guid tenantId)
    {
        Name = name;
        Email = email;
        TenantId = tenantId;
    }

    public void ChangePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
        Touch();
    }

    public void UpdateUser(string name)
    {
        Name = name;
        Touch();
    }

    private void Touch()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
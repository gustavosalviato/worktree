namespace WorkTree.Domain.Entities;

public class User : EntityBase
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public Guid TenantId { get; private set; }

    public User()
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

    public void Update(string name)
    {
        Name = name;
        Touch();
    }

    public void ChangeTenantId(Guid tenantId)
    {
        TenantId = tenantId;
        Touch();
    }

    private void Touch()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
namespace WebAPI.Test.Resources;

public class TenantIdentityManager
{
    private readonly WorkTree.Domain.Entities.Tenant _tenant;

    public TenantIdentityManager(WorkTree.Domain.Entities.Tenant tenant)
    {
        _tenant = tenant;
    }

    public Guid GetId() => _tenant.Id;
    public string GetName() => _tenant.Name;
    public string GetEmail() => _tenant.Email;
}
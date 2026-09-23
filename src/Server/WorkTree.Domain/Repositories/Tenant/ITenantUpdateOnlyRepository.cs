namespace WorkTree.Domain.Repositories.Tenant;

public interface ITenantUpdateOnlyRepository
{
    void Update(Entities.Tenant tenant);
}
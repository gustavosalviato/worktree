namespace WorkTree.Domain.Repositories.Tenant;

public interface ITenantWriteOnlyRepository
{
    Task AddAsync(Entities.Tenant tenant);
    void Update(Entities.Tenant tenant);
    void Delete(Entities.Tenant tenant);
}
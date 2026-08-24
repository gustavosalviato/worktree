using WorkTree.API.Entities;

namespace WorkTree.API.Contracts.Repositories;

public interface ITenantRepository
{
    void Create(Tenant tenant);
    void Update(Tenant tenant);
    void Delete(Tenant tenant);
    Tenant? FindById(Guid id);
    Tenant? FindByEmail(string email);
}
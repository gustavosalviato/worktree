using WorkTree.API.Entities;

namespace WorkTree.API.Contracts.Repositories;

public interface ITenantRepository
{
    Task CreateAsync(Tenant tenant);
    Task UpdateAsync(Tenant tenant);
    Task DeleteAsync(Tenant tenant);
    Task<Tenant?> FindByIdAsync(Guid id);
    Task<Tenant?> FindByEmailAsync(string email);
}
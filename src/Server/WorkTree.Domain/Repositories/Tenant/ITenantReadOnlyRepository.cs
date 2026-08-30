namespace WorkTree.Domain.Repositories.Tenant;

public interface ITenantReadOnlyRepository
{
    Task<Entities.Tenant?> FindByIdAsync(Guid id);
    Task<Entities.Tenant?> FindByEmailAsync(string email);
}
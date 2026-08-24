using WorkTree.API.Contracts.Repositories;
using WorkTree.API.Entities;

namespace WorkTree.API.Infra.Repositories;

public class TenantsRepository : ITenantRepository
{
    private readonly WorkTreeDbContext _context;

    public TenantsRepository(WorkTreeDbContext context) => _context = context;

    public void Create(Tenant tenant)
    {
        _context.Tenants.Add(tenant);
        _context.SaveChanges();
    }

    public void Update(Tenant tenant)
    {
        _context.Tenants.Update(tenant);
        _context.SaveChanges();
    }

    public void Delete(Tenant tenant)
    {
        _context.Tenants.Remove(tenant);
        _context.SaveChanges();
    }

    public Tenant? FindById(Guid id)
    {
        var tenant = _context.Tenants.FirstOrDefault(u => u.Id == id);

        return tenant;
    }

    public Tenant? FindByEmail(string email)
    {
        var tenant = _context.Tenants.FirstOrDefault(u => u.Email == email);

        return tenant;
    }
}
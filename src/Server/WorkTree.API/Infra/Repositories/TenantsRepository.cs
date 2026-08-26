using Microsoft.EntityFrameworkCore;
using WorkTree.API.Contracts.Repositories;
using WorkTree.API.Entities;

namespace WorkTree.API.Infra.Repositories;

public class TenantsRepository : ITenantRepository
{
    private readonly WorkTreeDbContext _context;

    public TenantsRepository(WorkTreeDbContext context) => _context = context;

    public async Task CreateAsync(Tenant tenant)
    {
        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Tenant tenant)
    {
        _context.Tenants.Update(tenant);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Tenant tenant)
    {
        _context.Tenants.Remove(tenant);
        await _context.SaveChangesAsync();
    }

    public async Task<Tenant?> FindByIdAsync(Guid id)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(u => u.Id == id);

        return tenant;
    }

    public Task<Tenant?> FindByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<Tenant?> FindByEmailAsync(string email)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(u => u.Email == email);

        return tenant;
    }
}
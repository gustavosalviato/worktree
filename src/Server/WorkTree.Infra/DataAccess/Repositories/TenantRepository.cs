using Microsoft.EntityFrameworkCore;
using WorkTree.Domain.Entities;
using WorkTree.Domain.Repositories.Tenant;

namespace WorkTree.Infra.DataAccess.Repositories;

internal sealed class TenantRepository : ITenantWriteOnlyRepository, ITenantReadOnlyRepository
{
    private readonly WorkTreeDbContext _context;

    public TenantRepository(WorkTreeDbContext context) => _context = context;

    public async Task AddAsync(Tenant tenant)
    {
        await _context.Tenants.AddAsync(tenant);
    }

    public void Update(Tenant tenant)
    {
        _context.Tenants.Update(tenant);
    }

    public void Delete(Tenant tenant)
    {
        _context.Tenants.Remove(tenant);
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
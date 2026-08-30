using Microsoft.EntityFrameworkCore;
using WorkTree.Domain.Entities;

namespace WorkTree.Infra.DataAccess;

internal class WorkTreeDbContext : DbContext
{
    public WorkTreeDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}
using WorkTree.Domain.Repositories;

namespace WorkTree.Infra.DataAccess;

internal class UnitOfWork : IUnitOfWork
{
    private readonly WorkTreeDbContext _context;

    public UnitOfWork(WorkTreeDbContext context)
    {
        _context = context;
    }

    public async Task CommitAsync() => await _context.SaveChangesAsync();
}
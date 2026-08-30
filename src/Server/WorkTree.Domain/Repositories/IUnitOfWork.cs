namespace WorkTree.Domain.Repositories;

public interface IUnitOfWork
{
    Task CommitAsync();
}
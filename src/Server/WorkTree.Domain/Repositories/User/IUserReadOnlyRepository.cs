namespace WorkTree.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    Task<Entities.User?> FindByEmailAsync(string email);
    Task<Entities.User?> FindByIdAsync(Guid id);
    Task<List<Entities.User>> FindManyAsync();
}
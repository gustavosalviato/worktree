using WorkTree.API.Entities;

namespace WorkTree.API.Contracts.Repositories;

public interface IUserRepository
{
    Task CreateAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task<User?> FindByEmailAsync(string email);
    Task<User?> FindByIdAsync(Guid id);
    Task<List<User>> FindManyAsync();
}
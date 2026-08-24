using WorkTree.API.Entities;

namespace WorkTree.API.Contracts.Repositories;

public interface IUserRepository
{
    void Create(User user);
    void Update(User user);
    void Delete(User user);
    User? FindByEmail(string email);
    User? FindById(Guid id);
    List<User> FindMany();
}
namespace WorkTree.Domain.Repositories.User;

public interface IUserWriteOnlyRepository
{
    Task AddAsync(Entities.User user);
    void Update(Entities.User user);
    void Delete(Entities.User user);
}
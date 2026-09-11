namespace WorkTree.Domain.Repositories.User;

public interface IUserUpdateOnlyRepository
{
    void UpdateProfile(Entities.User user);
     Task UpdatePassword(Guid userId, string password);
}
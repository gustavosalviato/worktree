using WorkTree.Domain.Entities;

namespace WorkTree.Domain.Identity;

public interface ILoggedUser
{
    Task<User> Get();
    Guid GetUserId();
}   
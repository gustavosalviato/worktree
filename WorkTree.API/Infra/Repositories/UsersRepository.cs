using WorkTree.API.Contracts.Repositories;
using WorkTree.API.Entities;

namespace WorkTree.API.Infra.Repositories;

public class UsersRepository : IUserRepository
{
    private readonly WorkTreeDbContext _context;

    public UsersRepository(WorkTreeDbContext context) => _context = context;

    public void Create(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void Delete(User user)
    {
        _context.Users.Remove(user);
        _context.SaveChanges();
    }

    public User? FindByEmail(string email)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);

        return user;
    }

    public User? FindById(Guid id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);

        return user;
    }

    public List<User> FindMany()
    {
        var users = _context.Users.ToList();

        return users;
    }
}
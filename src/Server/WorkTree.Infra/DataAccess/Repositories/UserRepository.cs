using Microsoft.EntityFrameworkCore;
using WorkTree.Domain.Entities;
using WorkTree.Domain.Repositories.User;

namespace WorkTree.Infra.DataAccess.Repositories;

internal sealed class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository
{
    private readonly WorkTreeDbContext _context;

    public UserRepository(WorkTreeDbContext context) => _context = context;

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }


    public void Update(User user)
    {
        _context.Users.Update(user);
    }

    public void Delete(User user)
    {
        _context.Users.Remove(user);
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        return user;
    }

    public async Task<User?> FindByIdAsync(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        return user;
    }

    public async Task<List<User>> FindManyAsync()
    {
        var users = await _context.Users.ToListAsync();

        return users;
    }
}
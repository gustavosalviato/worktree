using Microsoft.EntityFrameworkCore;
using WorkTree.API.Contracts.Repositories;
using WorkTree.API.Entities;

namespace WorkTree.API.Infra.Repositories;

public class UsersRepository : IUserRepository
{
    private readonly WorkTreeDbContext _context;

    public UsersRepository(WorkTreeDbContext context) => _context = context;

    public async Task CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
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
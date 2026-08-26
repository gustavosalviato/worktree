using Microsoft.EntityFrameworkCore;
using WorkTree.API.Contracts.Repositories;
using WorkTree.API.Entities;

namespace WorkTree.API.Infra.Repositories;

public class RefreshTokensRepository : IRefreshTokenRepository
{
    private readonly WorkTreeDbContext _dbContext;

    public RefreshTokensRepository(WorkTreeDbContext dbContext) => _dbContext = dbContext;

    public async Task CreateAsync(RefreshToken refreshToken)
    {
        _dbContext.Add(refreshToken);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(RefreshToken refreshToken)
    {
        _dbContext.Update(refreshToken);
        await _dbContext.SaveChangesAsync();
    }


    public async Task<RefreshToken?> GetTokenByHashedAsync(string hashedToken)
    {
        var refreshToken = await _dbContext.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.TokenHash == hashedToken);

        return refreshToken;
    }
}
using WorkTree.API.Entities;

namespace WorkTree.API.Contracts.Repositories;

public interface IRefreshTokenRepository
{
    Task CreateAsync(RefreshToken refreshToken);
    Task UpdateAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetTokenByHashedAsync(string hashedToken);
}
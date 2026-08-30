namespace WorkTree.Domain.Repositories.RefreshToken;

public interface IRefreshTokenWriteOnlyRepository
{
    Task CreateAsync(Entities.RefreshToken refreshToken);
    Task UpdateAsync(Entities.RefreshToken refreshToken);
    Task<Entities.RefreshToken?> GetTokenByHashedAsync(string hashedToken);
}
using WorkTree.API.Entities;

namespace WorkTree.API.Contracts.Services;

public interface ITokenService
{
    Task<(string acessToken, string refreshToken)> GenerateTokensAsync(User user);
    Task<(bool success, string? newAccessToken, string? newRefreshToken)> RefreshAsync(string refreshToken);
    Task RevokeRefreshTokenAsync(string refreshToken);
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    string HashToken(string token);
}
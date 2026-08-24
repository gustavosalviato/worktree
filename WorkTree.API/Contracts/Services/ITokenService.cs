using WorkTree.API.Entities;

namespace WorkTree.API.Contracts.Services;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken(User user);
    Task<(bool isValid, string? subId)> ValidateTokenAsync(string token);
}
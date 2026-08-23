using WorkTree.API.Entities;

namespace WorkTree.API.Contracts;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken(User user);
}
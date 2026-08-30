using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WorkTree.API.Contracts.Services;
using WorkTree.Domain.Entities;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace WorkTree.API.Infra.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    // private readonly IRefreshTokenRepository _repository;
    private readonly byte[] _key;


    // public TokenService(IConfiguration config, IRefreshTokenRepository repository)
    // {
    //     _config = config;
    //     _key = Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"] ?? "");
    //     // _repository = repository;
    // }

    public async Task<(string acessToken, string refreshToken)> GenerateTokensAsync(User user)
    {
        var accessToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken();

        var hashed = HashToken(refreshToken);

        var expiresAt = DateTime.UtcNow.AddDays(int.Parse(_config["Jwt:RefreshTokenExpirationDays"] ?? string.Empty));

        var rt = new RefreshToken(hashed, user.Id, expiresAt);

        // await _repository.CreateAsync(rt);

        return (accessToken, refreshToken);
    }

    public async Task<(bool success, string? newAccessToken, string? newRefreshToken)> RefreshAsync(string refreshToken)
    {
        var hashed = HashToken(refreshToken);

        // var rtEntity = await _repository.GetTokenByHashedAsync(hashed);

        // if (rtEntity == null || rtEntity.IsRevoked || rtEntity.IsExpired)
        //     return (false, null, null);

        // rtEntity.Revoke();

        // await _repository.UpdateAsync(rtEntity);

        var newRefreshToken = GenerateRefreshToken();
        var newHashed = HashToken(newRefreshToken);

        var expiresAt = DateTime.UtcNow.AddDays(int.Parse(_config["Jwt:RefreshTokenExpirationDays"] ?? string.Empty));

        // var rt = new RefreshToken(newHashed, rtEntity.UserId, expiresAt);

        // await _repository.CreateAsync(rt);

        // var newAccessToken = GenerateAccessToken(rtEntity.User);

        // return (true, newAccessToken, newRefreshToken);
        return (true, "asdasd", newRefreshToken);
    }


    public string GenerateAccessToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim("tenantId", user.TenantId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var credentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:AccessTokenExpirationMinutes"] ?? "15"));

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expires,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var random = RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(random);
    }

    public string HashToken(string token)
    {
        var bytes = Encoding.UTF8.GetBytes(token);
        var hash = SHA256.HashData(bytes);

        return Convert.ToHexString(hash);
    }

    public async Task<bool> RevokeRefreshTokenAsync(string refreshToken)
    {
        var hashed = HashToken(refreshToken);

        // var rtEntity = await _repository.GetTokenByHashedAsync(hashed);

        // if (rtEntity is null)
        //     return false;

        // rtEntity.Revoke();

        // await _repository.UpdateAsync(rtEntity);

        return true;
    }
}
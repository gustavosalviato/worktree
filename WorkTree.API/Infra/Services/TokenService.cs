using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WorkTree.API.Contracts;
using WorkTree.API.Entities;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace WorkTree.API.Infra.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly byte[] _key;


    public TokenService(IConfiguration config)
    {
        _config = config;
        _key = Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"] ?? "");
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

    public string GenerateRefreshToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var credentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddDays(int.Parse(_config["Jwt:RefreshTokenExpirationDays"] ?? "30"));

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

    public async Task<(bool isValid, string? subId)> ValidateTokenAsync(string token)
    {
        var tokenParameters = TokenHelper.BuildTokenValidationParameters(_config);
        var validTokenResult = await new JwtSecurityTokenHandler().ValidateTokenAsync(token, tokenParameters);

        if (!validTokenResult.IsValid)
            return (false, null);


        if (!validTokenResult.Claims.TryGetValue(ClaimTypes.NameIdentifier, out var subId))
        {
            return (false, null);
        }

        return (true, subId.ToString());
    }
}
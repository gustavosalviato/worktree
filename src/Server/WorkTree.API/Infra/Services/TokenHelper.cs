using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WorkTree.API.Infra.Services;

public static class TokenHelper
{
    public static TokenValidationParameters BuildTokenValidationParameters(IConfiguration config)
    {
        var jwtKey = config["Jwt:SecretKey"] ?? "";

        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["Jwt:Issuer"],
            ValidAudience = config["Jwt:Audience"],
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero,
        };
    }
}
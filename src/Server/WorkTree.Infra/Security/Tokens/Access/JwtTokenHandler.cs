using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using WorkTree.Domain.Entities;
using WorkTree.Domain.Security.Tokens;

namespace WorkTree.Infra.Security.Tokens.Access;

internal sealed class JwtTokenHandler : IAccessTokenGenerator
{
    private readonly uint _expirationInMinutes;
    private readonly string _signingKey;

    public JwtTokenHandler(uint expirationInMinutes, string signingKey)
    {
        _expirationInMinutes = expirationInMinutes;
        _signingKey = signingKey;
    }

    public string Generate(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.Name),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(_expirationInMinutes),
            SigningCredentials = new SigningCredentials(Credentials(), SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(claims)
        };


        var handler = new JsonWebTokenHandler();

        return handler.CreateToken(tokenDescriptor);
    }


    private SymmetricSecurityKey Credentials()
    {
        var keyBytes = Encoding.UTF8.GetBytes(_signingKey);

        return new SymmetricSecurityKey(keyBytes);
    }
}
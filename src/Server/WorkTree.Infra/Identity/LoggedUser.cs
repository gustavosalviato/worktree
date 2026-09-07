using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using WorkTree.Domain.Entities;
using WorkTree.Domain.Identity;
using WorkTree.Domain.Security.Tokens;
using WorkTree.Infra.DataAccess;

namespace WorkTree.Infra.Identity;

internal sealed class LoggedUser : ILoggedUser
{
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly WorkTreeDbContext _dbContext;

    public LoggedUser(IAccessTokenProvider accessTokenProvider, WorkTreeDbContext dbContext)
    {
        _accessTokenProvider = accessTokenProvider;
        _dbContext = dbContext;
    }

    public async Task<User> Get()
    {
        var userId = GetUserId();

        return await _dbContext.Users.AsNoTracking().FirstAsync(user => user.Id == userId);
    }

    public Guid GetUserId()
    {
        var accessToken = _accessTokenProvider.GetToken();

        var handler = new JsonWebTokenHandler();

        var jsonWebToken = handler.ReadJsonWebToken(accessToken);

        var subject = jsonWebToken.Claims.First(claim => claim.Type.Equals(JwtRegisteredClaimNames.Sub));

        return Guid.Parse(subject.Value);
    }
}
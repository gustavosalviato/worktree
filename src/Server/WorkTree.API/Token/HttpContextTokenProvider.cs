using WorkTree.Domain.Security.Tokens;

namespace WorkTree.API.Token;

internal sealed class HttpContextTokenProvider : IAccessTokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;


    public HttpContextTokenProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetToken()
    {
        var accessToken = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString()
            .Replace("Bearer ", string.Empty);

        return accessToken;
    }
}
namespace WorkTree.Domain.Security.Tokens;

public interface IAccessTokenProvider
{
    string GetToken();
}
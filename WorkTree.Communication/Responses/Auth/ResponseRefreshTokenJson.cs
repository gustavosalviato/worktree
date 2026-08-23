namespace WorkTree.Communication.Responses.Auth;

public class ResponseRefreshTokenJson
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
namespace WorkTree.Communication.Responses.Auth;

public class ResponseAuthenticateUserJson
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
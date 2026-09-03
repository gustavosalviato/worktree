namespace WorkTree.Communication.Requests.Auth;

public class RequestAuthenticateJson
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
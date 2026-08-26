namespace WorkTree.Communication.Requests.Auth;

public class RequestAuthenticateUserJson
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
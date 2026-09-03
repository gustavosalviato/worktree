namespace WebAPI.Test.Resources;

public class UserIdentityManager
{
    private readonly WorkTree.Domain.Entities.User _user;
    private readonly string _password;


    public UserIdentityManager(WorkTree.Domain.Entities.User user, string password)
    {
        _user = user;
        _password = password;
    }


    public Guid GetId() => _user.Id;
    public string GetName() => _user.Name;
    public string GetEmail() => _user.Email;
    public string GetPasswordHash() => _user.PasswordHash;
    public string GetPassword() => _password;
    public Guid GetTenantId() => _user.TenantId;
}
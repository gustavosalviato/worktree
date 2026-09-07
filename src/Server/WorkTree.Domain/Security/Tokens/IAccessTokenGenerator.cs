using WorkTree.Domain.Entities;

namespace WorkTree.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    string Generate(User user);
}
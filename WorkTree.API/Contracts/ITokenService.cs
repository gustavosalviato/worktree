using WorkTree.API.Entities;

namespace WorkTree.API.Contracts;

public interface ITokenService
{
    string Generate(User user);
}
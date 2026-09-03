using Bogus;
using CommonTestUtilities.Security;
using WorkTree.Domain.Entities;

namespace CommonTestUtilities.Entities;

public static class UserBuilder
{
    public static (User user, string password) Build()
    {
        var (password, passwordHash) = GenerateRandomPassword();

        var user = new Faker<User>()
            .RuleFor(request => request.Name, f => f.Person.FirstName)
            .RuleFor(request => request.Email, f => f.Internet.Email())
            .RuleFor(request => request.PasswordHash, f => passwordHash)
            .RuleFor(request => request.TenantId, f => f.Random.Guid());

        return (user, password);
    }


    private static (string password, string passwordHash) GenerateRandomPassword()
    {
        var passwordHasher = new PasswordHasherBuilder().Build();

        var password = new Faker().Internet.Password();

        return (password, passwordHasher.HashPassword(password));
    }
}
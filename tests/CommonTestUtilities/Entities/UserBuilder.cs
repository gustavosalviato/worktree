using Bogus;
using CommonTestUtilities.Security;
using WorkTree.Domain.Entities;

namespace CommonTestUtilities.Entities;

public static class UserBuilder
{
    public static User Build()
    {
        return new Faker<User>()
            .RuleFor(request => request.Name, f => f.Person.FirstName)
            .RuleFor(request => request.Email, f => f.Internet.Email())
            .RuleFor(request => request.PasswordHash, f => GenerateRandomPassword())
            .RuleFor(request => request.TenantId, f => f.Random.Guid());
    }


    private static string GenerateRandomPassword()
    {
        var passwordHasher = new PasswordHasherBuilder().Build();

        var password = new Faker().Internet.Password();

        return passwordHasher.HashPassword(password);
    }
}
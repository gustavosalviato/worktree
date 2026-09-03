using Bogus;
using CommonTestUtilities.Security;
using WorkTree.Domain.Entities;

namespace CommonTestUtilities.Entities;

public static class TenantBuilder
{
    public static Tenant Build()
    {
        return new Faker<Tenant>()
            .RuleFor(request => request.Name, f => f.Person.FirstName)
            .RuleFor(request => request.Email, f => f.Internet.Email());
    }
}
using Bogus;
using WorkTree.Communication.Requests.Tenants;

namespace CommonTestUtilities.Requests;

public class RequestCreateTenantJsonBuilder
{
    public static RequestCreateTenantJson Build()
    {
        return new Faker<RequestCreateTenantJson>()
            .RuleFor(request => request.Name, f => f.Person.FirstName)
            .RuleFor(request => request.Email, f => f.Internet.Email());
    }
}
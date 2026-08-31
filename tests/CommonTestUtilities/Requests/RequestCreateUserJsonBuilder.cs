using Bogus;
using WorkTree.Communication.Requests.Users;

namespace CommonTestUtilities.Requests;

public static class RequestCreateUserJsonBuilder
{
    public static RequestCreateUserJson Build()
    {
        return new Faker<RequestCreateUserJson>()
            .RuleFor(request => request.Name, f => f.Person.FirstName)
            .RuleFor(request => request.Email, f => f.Internet.Email())
            .RuleFor(request => request.Password, f => f.Internet.Password())
            .RuleFor(request => request.TenantId, f => f.Random.Guid());
    }
}
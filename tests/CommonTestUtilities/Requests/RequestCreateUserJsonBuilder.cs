using Bogus;
using WorkTree.Communication.Requests.Users;

namespace CommonTestUtilities.Requests;

public static class RequestCreateUserJsonBuilder
{
    public static RequestCreateUserJson Build(int passwordLength = 10)
    {
        return new Faker<RequestCreateUserJson>()
            .RuleFor(request => request.Name, f => f.Person.FirstName)
            .RuleFor(request => request.Email, f => f.Internet.Email())
            .RuleFor(request => request.Password, f => f.Internet.Password(passwordLength))
            .RuleFor(request => request.TenantId, f => f.Random.Guid());
    }
}
using Bogus;
using WorkTree.Communication.Requests.Users;

namespace CommonTestUtilities.Requests;

public static class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Build()
    {
        return new Faker<RequestUpdateUserJson>()
            .RuleFor(request => request.Name, f => f.Person.FirstName);
    }
}
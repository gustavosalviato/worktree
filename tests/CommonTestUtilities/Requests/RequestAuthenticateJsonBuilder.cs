using Bogus;
using WorkTree.Communication.Requests.Auth;

namespace CommonTestUtilities.Requests;

public static class RequestAuthenticateJsonBuilder
{
    public static RequestAuthenticateJson Build()
    {
        return new Faker<RequestAuthenticateJson>()
            .RuleFor(request => request.Email, f => f.Internet.Email())
            .RuleFor(request => request.Password, f => f.Internet.Password());
    }
}
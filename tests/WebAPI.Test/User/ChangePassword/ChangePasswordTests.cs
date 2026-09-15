using System.Net;
using CommonTestUtilities.Requests;
using Shouldly;
using WebAPI.Test.Resources;

namespace WebAPI.Test.User.ChangePassword;

public class ChangePasswordTests : BaseIntegrationTest
{
    private readonly UserIdentityManager _firstUser;

    private const string RequestUri = "/api/users/password";

    public ChangePasswordTests(WorkTreeApplicationFactory factory) : base(factory)
    {
        _firstUser = factory.FirstUser;
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();

        request.CurrentPassword = _firstUser.GetPassword();

        var response = await Put(RequestUri, request, _firstUser.GetAccessToken());

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}
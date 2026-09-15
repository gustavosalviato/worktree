using System.Net;
using Shouldly;
using WorkTree.Communication.Requests.Users;

namespace WebAPI.Test.User.ChangePassword;

public class InvalidTokenChangePasswordTests : BaseIntegrationTest
{
    private readonly string _tokenUserNotFound;

    private const string RequestUri = "/api/users/password";


    public InvalidTokenChangePasswordTests(WorkTreeApplicationFactory factory) : base(factory)
    {
        _tokenUserNotFound = factory.TokenUserNotFound;
    }


    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = new RequestChangePasswordJson();

        var response = await Put(RequestUri, request, "invalid-token");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var request = new RequestChangePasswordJson();

        var response = await Put(RequestUri, request, string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var request = new RequestChangePasswordJson();

        var response = await Put(RequestUri, request, _tokenUserNotFound);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
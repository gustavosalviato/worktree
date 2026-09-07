using System.Net;
using System.Text.Json;
using Shouldly;
using WebAPI.Test.Resources;

namespace WebAPI.Test.User.GetById;

public class GetUserByIdTests : BaseIntegrationTest
{
    private const string RequestUri = "/api/users/profile";

    private readonly UserIdentityManager _firstUser;

    public GetUserByIdTests(WorkTreeApplicationFactory factory) : base(factory)
    {
        _firstUser = factory.FirstUser;
    }


    [Fact]
    public async Task Success()
    {
        var response = await Get(RequestUri, _firstUser.GetAccessToken());

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().ShouldBe(_firstUser.GetName());
        responseData.RootElement.GetProperty("email").GetString().ShouldBe(_firstUser.GetEmail());
    }
}
using System.Net;
using System.Text.Json;
using Shouldly;
using WebAPI.Test.Resources;

namespace WebAPI.Test.User.GetAll;

public class GetAllUsersTests : BaseIntegrationTest
{
    private const string RequestUri = "/api/users";

    private readonly UserIdentityManager _firstUser;

    public GetAllUsersTests(WorkTreeApplicationFactory factory) : base(factory)
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

        responseData.RootElement.ValueKind.ShouldBe(JsonValueKind.Array);

        var seededUser = responseData.RootElement.EnumerateArray()
            .Single(element => element.GetProperty("email").GetString() == _firstUser.GetEmail());

        seededUser.GetProperty("name").GetString().ShouldBe(_firstUser.GetName());
        seededUser.GetProperty("tenantId").GetString().ShouldBe(_firstUser.GetTenantId().ToString());
    }
}

using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using Shouldly;
using WebAPI.Test.InlineData;
using WebAPI.Test.Resources;
using WorkTree.Communication.Requests.Auth;
using WorkTree.Exceptions;

namespace WebAPI.Test.Authentication.Authenticate.WithEmailAndPassword;

public class AuthenticateWithEmailAndPasswordTest : BaseIntegrationTest
{
    private readonly UserIdentityManager _firstUser;

    private const string RequestUri = "/api/auth/login";

    public AuthenticateWithEmailAndPasswordTest(WorkTreeApplicationFactory factory) : base(factory)
    {
        _firstUser = factory.FirstUser;
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestAuthenticateJson
        {
            Email = _firstUser.GetEmail(),
            Password = _firstUser.GetPassword(),
        };

        var response = await Post(RequestUri, request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("accessToken").GetString().ShouldBeEmpty();
        responseData.RootElement.GetProperty("refreshToken").GetString().ShouldBeEmpty();
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Validate_ShouldThrowException_WhenUserDoesNotExists(string culture)
    {
        var request = RequestAuthenticateJsonBuilder.Build();

        var response = await Post(RequestUri, request, culture);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedErrorMessage =
            ResourceMessagesException.ResourceManager.GetString("INVALID_CREDENTIALS", new CultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error =>
                error.GetString()!.Equals(expectedErrorMessage));
        });
    }
}
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CommonTestUtilities.Requests;
using Shouldly;
using WebAPI.Test.InlineData;
using WebAPI.Test.Resources;
using WorkTree.Communication.Requests.Auth;
using WorkTree.Exceptions;

namespace WebAPI.Test.Authentication.Authenticate.WithEmailAndPassword;

public class AuthenticateWithEmailAndPasswordTest : IClassFixture<WorkTreeApplicationFactory>
{
    private readonly HttpClient _httpClient;
    private readonly UserIdentityManager _firstUser;

    private const string REQUEST_URI = "/api/auth/login";

    public AuthenticateWithEmailAndPasswordTest(WorkTreeApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
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

        var response = await _httpClient.PostAsJsonAsync(REQUEST_URI, request);

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

        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(culture);

        var response = await _httpClient.PostAsJsonAsync(REQUEST_URI, request);

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
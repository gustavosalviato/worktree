using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using Shouldly;
using WebAPI.Test.InlineData;
using WebAPI.Test.Resources;
using WorkTree.Exceptions;

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

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Validate_ShouldThrowException_WhenUserDoesNotExists(string culture)
    {
        var request = RequestChangePasswordJsonBuilder.Build();

        request.CurrentPassword = _firstUser.GetPassword();

        var response = await Put(RequestUri, request, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        
        var responseData = await JsonDocument.ParseAsync(responseBody);
        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
        
        var expectedErrorMessage =
            ResourceMessagesException.ResourceManager.GetString("VALIDATION_ACCESS_TOKEN_REQUIRED", new CultureInfo(culture));
        
        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error =>
                error.GetString()!.Equals(expectedErrorMessage));
        });
    }
}
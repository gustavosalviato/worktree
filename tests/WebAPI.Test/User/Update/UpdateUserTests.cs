using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using WebAPI.Test.InlineData;
using WebAPI.Test.Resources;
using WorkTree.Communication.Requests.Users;
using WorkTree.Exceptions;

namespace WebAPI.Test.User.Update;

public class UpdateUserTests : BaseIntegrationTest
{
    private readonly UserIdentityManager _firstUser;


    private const string RequestUri = "/api/users/profile";

    public UpdateUserTests(WorkTreeApplicationFactory factory) : base(factory)
    {
        _firstUser = factory.FirstUser;
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestUpdateUserJson
        {
            Name = _firstUser.GetName(),
        };

        var response = await Put(RequestUri, request, _firstUser.GetAccessToken());

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        _firstUser.GetName().ShouldBe(request.Name);

        var wasUpdatedSuccessfully =
            await dbContext.Users.AnyAsync(user => user.Id == _firstUser.GetId() && user.Name == request.Name);

        wasUpdatedSuccessfully.ShouldBeTrue();
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Validate_ShouldThrowException_WhenUserNameIsEmpty(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        request.Name = string.Empty;

        var response = await Put(RequestUri, request, _firstUser.GetAccessToken(), culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedErrorMessage =
            ResourceMessagesException.ResourceManager.GetString("VALIDATION_NAME_REQUIRED",
                new CultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error =>
                error.GetString()!.Equals(expectedErrorMessage));
        });
    }
}
using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using WebAPI.Test.InlineData;
using WebAPI.Test.Resources;
using WorkTree.Exceptions;

namespace WebAPI.Test.User.Create;

public class CreateUserTests : BaseIntegrationTest
{
    private const string RequestUri = "/api/users";

    private readonly TenantIdentityManager _firstTenant;

    public CreateUserTests(WorkTreeApplicationFactory factory) : base(factory)
    {
        _firstTenant = factory.FirstTenant;
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestCreateUserJsonBuilder.Build();

        request.TenantId = _firstTenant.GetId();

        var response = await Post(RequestUri, request);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().ShouldBe(request.Name);
        responseData.RootElement.GetProperty("email").GetString().ShouldBe(request.Email);
        responseData.RootElement.GetProperty("tenantId").GetString().ShouldBe(request.TenantId.ToString());

        var userExists =
            await dbContext.Users.AnyAsync(user => user.Name.Equals(request.Name) && user.Email.Equals(request.Email));

        userExists.ShouldBeTrue();
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Validate_ShouldBeAnErrorResponse_WhenNameIsEmpty(string culture)
    {
        var request = RequestCreateUserJsonBuilder.Build();

        request.Name = string.Empty;
        request.TenantId = _firstTenant.GetId();

        var response = await Post(RequestUri, request, culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedErrorMessage =
            ResourceMessagesException.ResourceManager.GetString("VALIDATION_NAME_REQUIRED", new CultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error =>
                error.GetString()!.Equals(expectedErrorMessage));
        });


        var userExists =
            await dbContext.Users.AnyAsync(user => user.Name.Equals(request.Name) && user.Email.Equals(request.Email));

        userExists.ShouldBeFalse();
    }
}
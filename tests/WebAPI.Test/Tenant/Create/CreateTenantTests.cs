using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using WebAPI.Test.InlineData;
using WorkTree.Exceptions;

namespace WebAPI.Test.Tenant.Create;

public class CreateTenantTests : BaseIntegrationTest
{
    private const string RequestUri = "/api/tenants";

    public CreateTenantTests(WorkTreeApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestCreateTenantJsonBuilder.Build();

        var response = await Post(RequestUri, request);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().ShouldBe(request.Name);
        responseData.RootElement.GetProperty("email").GetString().ShouldBe(request.Email);

        var tenantExists =
            await dbContext.Tenants.AnyAsync(tenant =>
                tenant.Name.Equals(request.Name) && tenant.Email.Equals(request.Email));

        tenantExists.ShouldBeTrue();
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Validate_ShouldBeAnErrorResponse_WhenNameIsEmpty(string culture)
    {
        var request = RequestCreateTenantJsonBuilder.Build();

        request.Name = string.Empty;

        var response = await Post(RequestUri, request, culture: culture);

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

        var tenantExists =
            await dbContext.Tenants.AnyAsync(user =>
                user.Name.Equals(request.Name) && user.Email.Equals(request.Email));

        tenantExists.ShouldBeFalse();
    }
}
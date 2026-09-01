using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CommonTestUtilities.Requests;
using Shouldly;

namespace WebAPI.Test.User.Create;

public class CreateUserTests : IClassFixture<WorkTreeApplicationFactory>
{
    private readonly HttpClient _httpClient;
    private readonly WorkTreeApplicationFactory _factory;
    private const string REQUEST_URI = "/api/users";

    public CreateUserTests(WorkTreeApplicationFactory factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task Success()
    {
        var tenant = await _factory.SeedTenantAsync();

        var request = RequestCreateUserJsonBuilder.Build();

        request.TenantId = tenant.Id;
        var response = await _httpClient.PostAsJsonAsync(REQUEST_URI, request);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);


        responseData.RootElement.GetProperty("name").GetString().ShouldBe(request.Name);
        responseData.RootElement.GetProperty("email").GetString().ShouldBe(request.Email);
        responseData.RootElement.GetProperty("tenantId").GetString().ShouldBe(request.TenantId.ToString());
    }

    [Theory]
    [InlineData("en")]
    [InlineData("pt-BR")]
    public async Task Validate_ShouldBeAnErrorResponse_WhenNameIsEmpty(string culture)
    {
        var tenant = await _factory.SeedTenantAsync();

        var request = RequestCreateUserJsonBuilder.Build();

        request.Name = string.Empty;
        request.TenantId = tenant.Id;

        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(culture);

        var response = await _httpClient.PostAsJsonAsync(REQUEST_URI, request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error => error.GetString()!.Equals("Name could not be empty."));
        });
    }
}
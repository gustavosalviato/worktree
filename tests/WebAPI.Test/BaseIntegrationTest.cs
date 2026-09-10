using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Test.Resources;
using WorkTree.Infra.DataAccess;

namespace WebAPI.Test;

public abstract class BaseIntegrationTest : IClassFixture<WorkTreeApplicationFactory>, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly IServiceScope _scope;

    internal readonly WorkTreeDbContext dbContext;

    public BaseIntegrationTest(WorkTreeApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();

        _scope = factory.Services.CreateScope();
        dbContext = _scope.ServiceProvider.GetRequiredService<WorkTreeDbContext>();
    }


    protected async Task<HttpResponseMessage> Post(string uri, object request, string accessToken = "", string culture = "en")
    {
        AssignRequestCulture(culture);
        AuthorizeRequest(accessToken);

        return await _httpClient.PostAsJsonAsync(uri, request);
    }
    
    protected async Task<HttpResponseMessage> Put(string uri, object request, string accessToken = "", string culture = "en")
    {
        AssignRequestCulture(culture);
        AuthorizeRequest(accessToken);

        return await _httpClient.PutAsJsonAsync(uri, request);
    }

    protected async Task<HttpResponseMessage> Get(string uri, string accessToken, string culture = "en")
    {
        AssignRequestCulture(culture);
        AuthorizeRequest(accessToken);

        return await _httpClient.GetAsync(uri);
    }

    private void AssignRequestCulture(string culture)
    {
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(culture);
    }

    private void AuthorizeRequest(string accessToken)
    {
        if (!string.IsNullOrEmpty(accessToken))
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }


    public void Dispose()
    {
        _scope?.Dispose();
        dbContext?.Dispose();
    }
}
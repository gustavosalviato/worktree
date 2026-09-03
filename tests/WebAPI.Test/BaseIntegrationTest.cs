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


    protected async Task<HttpResponseMessage> Post(string uri, object request, string culture = "en")
    {
        AssignRequestCulture(culture);

        return await _httpClient.PostAsJsonAsync(uri, request);
    }

    private void AssignRequestCulture(string culture)
    {
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(culture);
    }


    public void Dispose()
    {
        _scope?.Dispose();
        dbContext?.Dispose();
    }
}
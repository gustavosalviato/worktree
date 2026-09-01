using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace WebAPI.Test;

public class WorkTreeApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlBuilder;

    public WorkTreeApplicationFactory()
    {
        _postgreSqlBuilder = new PostgreSqlBuilder("postgres:16").WithDatabase("worktree").Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Tests").ConfigureAppConfiguration((_, configuration) =>
        {
            var parameters = new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = _postgreSqlBuilder.GetConnectionString(),
            };
            configuration.AddInMemoryCollection(parameters);
        });
    }


    public async Task InitializeAsync()
    {
        await _postgreSqlBuilder.StartAsync();
    }

    public Task DisposeAsync()
    {
        return _postgreSqlBuilder.StopAsync();
    }
}
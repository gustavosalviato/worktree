using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using WebAPI.Test.Resources;
using WorkTree.Domain.Entities;
using WorkTree.Domain.Security.PasswordHashing;
using WorkTree.Infra.DataAccess;

namespace WebAPI.Test;

public class WorkTreeApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public UserIdentityManager FirstUser { get; private set; }
    public TenantIdentityManager FirstTenant { get; private set; }  

    private readonly PostgreSqlContainer _postgreSqlContainer;

    public WorkTreeApplicationFactory()
    {
        _postgreSqlContainer = new PostgreSqlBuilder("postgres:16").WithDatabase("worktree").Build();
    }


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Tests").ConfigureAppConfiguration((_, configuration) =>
        {
            var parameters = new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = _postgreSqlContainer.GetConnectionString(),
            };
            configuration.AddInMemoryCollection(parameters);
        });
    }

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();

        var tenant = await SeedTenant();
        var (user, password) = await SeedUser(tenant.Id);

        FirstUser = new UserIdentityManager(user, password);
        FirstTenant = new TenantIdentityManager(tenant);
    }

    private async Task<(WorkTree.Domain.Entities.User user, string password)> SeedUser(Guid tenantId)
    {
        await using var scope = Services.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<WorkTreeDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        var (user, password) = UserBuilder.Build();

        var passwordHash = passwordHasher.HashPassword(password);

        user.ChangePassword(passwordHash);
        user.ChangeTenantId(tenantId);

        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        return (user, password);
    }

    private async Task<Tenant> SeedTenant()
    {
        await using var scope = Services.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<WorkTreeDbContext>();
        var tenant = TenantBuilder.Build();

        await dbContext.Tenants.AddAsync(tenant);
        await dbContext.SaveChangesAsync();

        return tenant;
    }


    public new async Task DisposeAsync()
    {
        await _postgreSqlContainer.StopAsync();
        await _postgreSqlContainer.DisposeAsync();
    }
}
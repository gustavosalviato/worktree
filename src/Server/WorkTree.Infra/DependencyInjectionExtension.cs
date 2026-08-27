using Microsoft.Extensions.DependencyInjection;
using WorkTree.Domain.Security.PasswordHashing;
using WorkTree.Infra.Security.PasswordHashing;

namespace WorkTree.Infra;

public static class DependencyInjectionExtension
{
    extension(IServiceCollection services)
    {
        public void AddInfrastructure()
        {
            services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();
        }
    }
}
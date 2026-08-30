using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkTree.Domain.Repositories;
using WorkTree.Domain.Repositories.RefreshToken;
using WorkTree.Domain.Repositories.Tenant;
using WorkTree.Domain.Repositories.User;
using WorkTree.Domain.Security.PasswordHashing;
using WorkTree.Infra.DataAccess;
using WorkTree.Infra.DataAccess.Repositories;
using WorkTree.Infra.Security.PasswordHashing;

namespace WorkTree.Infra;

public static class DependencyInjectionExtension
{
    extension(IServiceCollection services)
    {
        public void AddInfrastructure(IConfiguration configuration)
        {
            services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();
            
            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();
            
            services.AddScoped<ITenantWriteOnlyRepository, TenantRepository>();
            services.AddScoped<ITenantReadOnlyRepository, TenantRepository>();
            
            services.AddScoped<IRefreshTokenWriteOnlyRepository, RefreshTokenRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<WorkTreeDbContext>(option =>
            {
                option.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });
        }
    }
}
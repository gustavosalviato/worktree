using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkTree.Domain.Repositories;
using WorkTree.Domain.Repositories.RefreshToken;
using WorkTree.Domain.Repositories.Tenant;
using WorkTree.Domain.Repositories.User;
using WorkTree.Domain.Security.PasswordHashing;
using WorkTree.Domain.Security.Tokens;
using WorkTree.Infra.DataAccess;
using WorkTree.Infra.DataAccess.Repositories;
using WorkTree.Infra.Security.PasswordHashing;
using WorkTree.Infra.Security.Tokens.Access;

namespace WorkTree.Infra;

public static class DependencyInjectionExtension
{
    extension(IServiceCollection services)
    {
        public void AddInfrastructure(IConfiguration configuration)
        {
            services.AddRepositories();
            services.AddTokensHandlers(configuration);

            services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();

            services.AddDbContext<WorkTreeDbContext>(option =>
            {
                option.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });
        }

        private void AddRepositories()
        {
            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();

            services.AddScoped<ITenantWriteOnlyRepository, TenantRepository>();
            services.AddScoped<ITenantReadOnlyRepository, TenantRepository>();

            services.AddScoped<IRefreshTokenWriteOnlyRepository, RefreshTokenRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private void AddTokensHandlers(IConfiguration configuration)
        {
            services.AddScoped<IAccessTokenGenerator>(provider =>
            {
                var signinKey = configuration.GetValue<string>("Jwt:SecretKey")!;
                var accessTokenExpirationInMinutes = configuration.GetValue<uint>("Jwt:AccessTokenExpirationMinutes");

                return new JwtTokenHandler(accessTokenExpirationInMinutes, signinKey);
            });
        }
    }
}
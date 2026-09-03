using Microsoft.Extensions.DependencyInjection;
using WorkTree.Application.UseCases.Authentication.Authenticate.WithEmailAndPassword;
using WorkTree.Application.UseCases.Tenant.Create;
using WorkTree.Application.UseCases.Tenant.Delete;
using WorkTree.Application.UseCases.Tenant.GetById;
using WorkTree.Application.UseCases.Tenant.Update;
using WorkTree.Application.UseCases.User.Create;
using WorkTree.Application.UseCases.User.Delete;
using WorkTree.Application.UseCases.User.GetAll;
using WorkTree.Application.UseCases.User.GetById;
using WorkTree.Application.UseCases.User.Update;

namespace WorkTree.Application;

public static class DependencyInjectionExtension
{
    extension(IServiceCollection services)
    {
        public void AddApplication()
        {
            services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
            services.AddScoped<IDeleteUserUseCase, DeleteUserUseCase>();
            services.AddScoped<IGetAllUsersUseCase, GetAllUsersUseCase>();
            services.AddScoped<IGetUserByIdUseCase, GetUserByIdUseCase>();
            services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();

            services.AddScoped<ICreateTenantUseCase, CreateTenantUseCase>();
            services.AddScoped<IUpdateTenantUseCase, UpdateTenantUseCase>();
            services.AddScoped<IDeleteTenantUseCase, DeleteTenantUseCase>();
            services.AddScoped<IGetTenantByIdUseCase, GetTenantByIdUseCase>();

            services
                .AddScoped<IAuthenticateWithEmailAndPasswordUserUseCase, AuthenticateWithEmailAndPasswordUserUseCase>();
        }
    }
}
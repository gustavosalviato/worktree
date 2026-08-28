using Microsoft.Extensions.DependencyInjection;
using WorkTree.Application.UseCases.User.Create;

namespace WorkTree.Application;

public static class DependencyInjectionExtension
{
    extension(IServiceCollection services)
    {
        public void AddApplication()
        {
            services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
        }
    }
}
using WorkTree.API.Contracts;
using WorkTree.API.Entities;
using WorkTree.API.Filters;
using WorkTree.API.Infra;
using WorkTree.API.Infra.Repositories;
using WorkTree.API.UseCases.Users.Create;
using WorkTree.API.UseCases.Users.Update;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkTree.API.UseCases.Tenants.Create;
using WorkTree.API.UseCases.Tenants.Delete;
using WorkTree.API.UseCases.Tenants.Update;
using WorkTree.API.UseCases.Users.Delete;
using WorkTree.API.UseCases.Users.GetAll;
using WorkTree.API.UseCases.Users.GetById;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddMvc(option => option.Filters.Add(typeof(ExceptionFilter)));

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<CreateUserUseCase>();
builder.Services.AddScoped<UpdateUserUseCase>();
builder.Services.AddScoped<DeleteUserUseCase>();
builder.Services.AddScoped<GetUserByIdUseCase>();
builder.Services.AddScoped<GetAllUsersUseCase>();

builder.Services.AddScoped<CreateTenantUseCase>();
builder.Services.AddScoped<UpdateTenantUseCase>();
builder.Services.AddScoped<DeleteTenantUseCase>();

builder.Services.AddScoped<IUserRepository, UsersRepository>();
builder.Services.AddScoped<ITenantRepository, TenantsRepository>();

builder.Services.AddDbContext<WorkTreeDbContext>(option =>
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});


var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
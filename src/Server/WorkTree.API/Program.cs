using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WorkTree.API.Filters;
using WorkTree.API.Converters;
using WorkTree.API.Infra.Services;
using WorkTree.Application;
using WorkTree.Infra;
using WorkTree.Infra.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new StringConverter()));
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var jwtKey = builder.Configuration["Jwt:SecretKey"] ?? "";

    options.TokenValidationParameters = TokenHelper.BuildTokenValidationParameters(builder.Configuration);
});

builder.Services.AddAuthorization();

builder.Services.AddMvc(option => option.Filters.Add(typeof(ExceptionFilter)));

// builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
// builder.Services.AddScoped<CreateUserUseCase>();
// builder.Services.AddScoped<UpdateUserUseCase>();
// builder.Services.AddScoped<DeleteUserUseCase>();
// builder.Services.AddScoped<GetUserByIdUseCase>();
// builder.Services.AddScoped<GetAllUsersUseCase>();
//
// builder.Services.AddScoped<CreateTenantUseCase>();
// builder.Services.AddScoped<UpdateTenantUseCase>();
// builder.Services.AddScoped<DeleteTenantUseCase>();
// builder.Services.AddScoped<GetTenantByIdUseCase>();
//
// builder.Services.AddScoped<AuthenticateUserUseCase>();
// builder.Services.AddScoped<RefreshTokenUseCase>();
// builder.Services.AddScoped<LogoutUserUseCase>();
//
// builder.Services.AddScoped<IUserRepository, UsersRepository>();
// builder.Services.AddScoped<ITenantRepository, TenantsRepository>();
// builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokensRepository>();
// builder.Services.AddScoped<ITokenService, TokenService>();


builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

await ExecuteMigrations();

app.MapControllers();

app.Run();

async Task ExecuteMigrations()
{
    await using var scope = app.Services.CreateAsyncScope();

    await DatabaseMigration.ExecuteMigrations(scope.ServiceProvider);
}


public partial class Program
{
}
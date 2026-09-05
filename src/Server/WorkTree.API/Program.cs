using System.Globalization;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using WorkTree.API.Filters;
using WorkTree.API.Converters;
using WorkTree.Application;
using WorkTree.Communication.Responses;
using WorkTree.Domain.Repositories.User;
using WorkTree.Exceptions;
using WorkTree.Infra;
using WorkTree.Infra.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new StringConverter()));
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen((options) =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter only you access token, Swagger will add 'Bearer' automatically",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
    });

    options.AddSecurityRequirement(openApiDocument =>
    {
        return new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecuritySchemeReference("Bearer", openApiDocument),
                []
            }
        };
    });
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddAuthorization();

builder.Services.AddMvc(option => option.Filters.Add(typeof(ExceptionFilter)));

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>
    {
        new CultureInfo("en"),
        new CultureInfo("pt-BR"),
    };

    options.DefaultRequestCulture = new RequestCulture("en");

    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new AcceptLanguageHeaderRequestCultureProvider(),
    };
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    var signinKey = builder.Configuration.GetValue<string>("Jwt:SecretKey")!;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signinKey)),
        ClockSkew = TimeSpan.Zero,
    };

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var subject = context.Principal?.FindFirstValue(JwtRegisteredClaimNames.Sub) ??
                          context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);


            if (!Guid.TryParse(subject, out var userId))
            {
                context.Fail("Invalid token subject.");

                return;
            }

            var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserReadOnlyRepository>();

            var userExists = await userRepository.FindAnyByIdAsync(userId);

            if (!userExists)
                context.Fail("User not found.");
        },

        OnChallenge = async context =>
        {
            context.HandleResponse();

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            context.Response.ContentType = "application/json";

            var response = context.AuthenticateFailure switch
            {
                null => new ResponseErrorMessagesJson(ResourceMessagesException.VALIDATION_ACCESS_TOKEN_REQUIRED),
                SecurityTokenExpiredException => new ResponseErrorMessagesJson(message: "Token expired",
                    accessTokenExpired: true),
                _ => new ResponseErrorMessagesJson(ResourceMessagesException.VALIDATION_RESOURCE_ACCESS_DENIED),
            };

            await context.Response.WriteAsJsonAsync(response);
        },
    };
});

var app = builder.Build();

var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();

app.UseRequestLocalization(localizationOptions.Value);


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
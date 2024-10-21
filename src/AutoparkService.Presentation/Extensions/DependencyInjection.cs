using System.Security.Cryptography;
using Asp.Versioning;
using AutoparkService.Presentation.Middleware;
using AutoparkService.Presentation.OpenApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AutoparkService.Presentation.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers();
        services.ConfigureApiVersioning();

        services.ConfigureAuthentication(configuration);
        services.AddAuthorization();

        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.ConfigureOpenApi();

        return services;
    }

    private static IServiceCollection ConfigureApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(
            options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            }).AddApiExplorer(
            options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        return services;
    }

    private static IServiceCollection ConfigureAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
            .AddJwtBearer(
                options =>
                {
                    options.MetadataAddress = configuration.GetValue<string>("IdentityOIDC")!;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                    };
                });

        return services;
    }

    private static IServiceCollection ConfigureOpenApi(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.ConfigureOptions<ConfigureSwaggerGenOptions>();

        return services;
    }
}

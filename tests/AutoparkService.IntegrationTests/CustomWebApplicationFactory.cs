using System.Data.Common;
using AutoparkService.Infrastructure.Contexts;
using AutoparkService.IntegrationTests.JwtTokenGeneration;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;

namespace AutoparkService.IntegrationTests;

public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>,
        IAsyncLifetime
{
    private const string TestingEnvironment = "Testing";

    private readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder()
        .WithImage("postgres:16")
        .WithDatabase("autopark")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithPortBinding(5432, true)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("pg_isready"))
        .Build();

    private Respawner _respawner = null!;
    private DbConnection _dbConnection = null!;

    public async Task ResetDatabase()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();
        _dbConnection = new NpgsqlConnection(_postgresContainer.GetConnectionString());
        using var scope = Services.CreateScope();

        var authContextInitializer = scope.ServiceProvider.GetRequiredService<AutoParkDbContextInitializer>();
        await authContextInitializer.InitializeAsync();
        await authContextInitializer.SeedAsync();

        await InitializeRespawnerAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbConnection.DisposeAsync();
        await _postgresContainer.StopAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(TestingEnvironment);
        builder.ConfigureTestServices(
            services =>
            {
                // AuthContext
                var authContextDescriptor = services
                    .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<AutoParkDbContext>));

                if (authContextDescriptor is not null)
                {
                    services.Remove(authContextDescriptor);
                }

                services.AddDbContext<AutoParkDbContext>(
                    options =>
                        options.UseNpgsql(_postgresContainer.GetConnectionString()));

                // Authentication
                services.Configure<JwtBearerOptions>(
                    JwtBearerDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.MetadataAddress = default!;
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidIssuer = JwtTokenProvider.Issuer,
                            ValidateAudience = false,
                            IssuerSigningKey = JwtTokenProvider.SecurityKey,
                        };
                    });
            });
    }

    private async Task InitializeRespawnerAsync()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(
            _dbConnection,
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude =
                [
                    "public",
                ],
            });
    }
}

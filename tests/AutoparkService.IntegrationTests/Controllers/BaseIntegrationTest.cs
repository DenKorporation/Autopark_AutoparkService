using System.Net.Http.Headers;
using AutoparkService.Domain.Constants;
using AutoparkService.Infrastructure.Contexts;
using AutoparkService.IntegrationTests.JwtTokenGeneration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AutoparkService.IntegrationTests.Controllers;

public abstract class BaseIntegrationTest
    : IClassFixture<CustomWebApplicationFactory>,
        IDisposable
{
    private const string BasePath = "api/v1.0";
    private static string? _authenticationToken;

    private readonly CustomWebApplicationFactory _factory;
    private readonly IServiceScope _scope;

    protected BaseIntegrationTest(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _scope = factory.Services.CreateScope();
        TestDbContext = _scope.ServiceProvider.GetRequiredService<AutoParkDbContext>();
        Client = factory.CreateClient();

        if (_authenticationToken is null)
        {
            _authenticationToken = AdminJwtToken;
        }

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authenticationToken);

        if (Client.BaseAddress is not null)
        {
            Client.BaseAddress = new Uri(Client.BaseAddress.AbsoluteUri + BasePath);
        }
    }

    protected AutoParkDbContext TestDbContext { get; }
    protected HttpClient Client { get; }

    private static string AdminJwtToken =>
        new TestJwtToken().WithRole(Roles.Administrator).Build();

    public void Dispose()
    {
        Client.Dispose();
        TestDbContext.Dispose();
        _scope.Dispose();
    }

    protected async Task AddEntitiesToDbAsync<T>(List<T> entities)
        where T : class
    {
        TestDbContext.AddRange(entities);
        await TestDbContext.SaveChangesAsync();
    }

    protected async Task AddEntityToDbAsync<T>(T entity)
        where T : class
    {
        TestDbContext.Add(entity);
        await TestDbContext.SaveChangesAsync();
    }
}

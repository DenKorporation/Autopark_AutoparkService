using AutoparkService.Infrastructure.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutoparkService.Infrastructure.Extensions;

public static class DatabaseExtensions
{
    public static async Task<IHost> InitializeDatabaseAsync(
        this IHost app,
        CancellationToken cancellationToken = default)
    {
        using var scope = app.Services.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<AutoParkDbContextInitializer>();

        await initializer.InitializeAsync(cancellationToken);

        await initializer.SeedAsync(cancellationToken);

        return app;
    }
}

using System.Reflection;
using AutoparkService.Application.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutoparkService.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var applicationAssembly = Assembly.GetExecutingAssembly();

        services.ConfigureMediatR(applicationAssembly);
        services.ConfigureMapping(applicationAssembly);
        services.ConfigureValidation(applicationAssembly);

        return services;
    }

    private static IServiceCollection ConfigureMediatR(this IServiceCollection services, Assembly assembly)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        return services;
    }

    private static IServiceCollection ConfigureMapping(this IServiceCollection services, Assembly assembly)
    {
        services.AddAutoMapper(assembly);

        return services;
    }

    private static IServiceCollection ConfigureValidation(this IServiceCollection services, Assembly assembly)
    {
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}

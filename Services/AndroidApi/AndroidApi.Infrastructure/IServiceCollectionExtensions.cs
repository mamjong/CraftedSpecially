using AndroidApi.Application.Services;
using AndroidApi.Infrastructure.Services;
using CraftedSpecially.AndroidApi.Application.Features.SeeProductCatalog;
using CraftedSpecially.AndroidApi.Infrastructure.Agents;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace CraftedSpecially.AndroidApi.Infrastructure;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services) => services
            .AddAgents()
            .AddDependencies();

    private static IServiceCollection AddDependencies(this IServiceCollection services) => services
            .AddScoped<IProductCatalogService, ProductCatalogService>()
            .AddScoped<RegisterCatalogProduct>();

    private static IServiceCollection AddAgents(this IServiceCollection services)
    {
        Console.WriteLine($"{Environment.GetEnvironmentVariable("CATALOGUS_ADDRESS")}:{Environment.GetEnvironmentVariable("CATALOGUS_PORT")}");

        services
            .AddRefitClient<ICatalogusApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri($"{Environment.GetEnvironmentVariable("CATALOGUS_ADDRESS")}:{Environment.GetEnvironmentVariable("CATALOGUS_PORT")}"));

        return services;
    }
}
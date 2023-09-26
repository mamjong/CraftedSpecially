using CraftedSpecially.Application.Common.Interfaces;
using CraftedSpecially.Catalog.Application.Features.ProductRegistration;
using CraftedSpecially.Catalog.Application.Services;
using CraftedSpecially.Catalog.Domain.Aggregates.ProductAggregate;
using CraftedSpecially.Catalog.Domain.Aggregates.ProductAggregate.Commands;
using CraftedSpecially.Catalog.Application.Interfaces;
using CraftedSpecially.Catalog.Infrastructure.Persistence.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services
    )
    {
        return services
            .AddDatabase()
            .AddDependencies();
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services
            .AddDbContext<CatalogDbContext>(options =>
            {
                options.UseMySQL("server=mysql;database=db;user=user;password=password");
            });

        return services;
    }

    private static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        return services
            .AddTransient<IProductRepository, EFProductRepository>()
            .AddTransient<IProductService, ProductService>()
            .AddTransient<ICommandHandler<RegisterProductCommand>, RegisterProductHandler>();
    }
}
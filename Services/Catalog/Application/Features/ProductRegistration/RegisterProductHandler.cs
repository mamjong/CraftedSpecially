using CraftedSpecially.Application.Common.Interfaces;
using CraftedSpecially.Catalog.Application.Interfaces;
using CraftedSpecially.Catalog.Domain.Aggregates.ProductAggregate;
using CraftedSpecially.Catalog.Domain.Aggregates.ProductAggregate.Commands;

namespace CraftedSpecially.Catalog.Application.Features.ProductRegistration;

public class RegisterProductHandler : ICommandHandler<RegisterProductCommand>
{
    private readonly IProductService _productService;
    private readonly IProductRepository _productRepository;
    private readonly Instrumentation _instrumentation;

    public RegisterProductHandler(
        IProductService productService,
        IProductRepository productRepository, 
        Instrumentation instrumentation)
    {
        _productService = productService;
        _productRepository = productRepository;
        _instrumentation = instrumentation;
    }

    public async ValueTask Handle(RegisterProductCommand command)
    {
        using var activity = _instrumentation.ActivitySource.StartActivity("RegisterProductHandler");
        
        var product = new Product();

        await product.RegisterProductAsync(command, _productService);

        if (product.Name.StartsWith("slow"))
        {
            await PerformExtremelyComplexBusinessLogic();
        }

        await _productRepository.AddAsync(product);

        var domainEvents = product.GetDomainEvents();
    }
    
    private async Task PerformExtremelyComplexBusinessLogic()
    {
        await Task.Delay(1000);
    }
}
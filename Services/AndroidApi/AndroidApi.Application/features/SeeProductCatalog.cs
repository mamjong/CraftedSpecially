using AndroidApi.Application.Services;
using CraftedSpecially.AndroidApi.Domain;

namespace CraftedSpecially.AndroidApi.Application.Features.SeeProductCatalog;

public class SeeProductCatalog
{
    private readonly IProductCatalogService productCatalogService;

    public SeeProductCatalog(IProductCatalogService productCatalogService)
    {
        this.productCatalogService = productCatalogService;
    }

    public Task<IEnumerable<ProductCatalogItem>> GetCatalogProduct()
    {
        return productCatalogService.GetCatalogItems();
    }
}
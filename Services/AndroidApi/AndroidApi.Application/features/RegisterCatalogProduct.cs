using AndroidApi.Application.Services;

namespace CraftedSpecially.AndroidApi.Application.Features.SeeProductCatalog;

public class RegisterCatalogProduct
{
    private readonly IProductCatalogService productCatalogService;

    public RegisterCatalogProduct(IProductCatalogService productCatalogService)
    {
        this.productCatalogService = productCatalogService;
    }

    public async Task SendRegistration()
    {
        await productCatalogService.CreateProductCatalogItem();
    }
}
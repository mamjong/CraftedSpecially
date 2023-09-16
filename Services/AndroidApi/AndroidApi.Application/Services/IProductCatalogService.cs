using CraftedSpecially.AndroidApi.Domain;

namespace AndroidApi.Application.Services;

public interface IProductCatalogService
{
    Task<IEnumerable<ProductCatalogItem>> GetCatalogItems();
}
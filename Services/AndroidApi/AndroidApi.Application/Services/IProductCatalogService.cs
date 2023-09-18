using CraftedSpecially.AndroidApi.Domain;

namespace AndroidApi.Application.Services;

public interface IProductCatalogService
{
    Task CreateProductCatalogItem(string name, string description);
}
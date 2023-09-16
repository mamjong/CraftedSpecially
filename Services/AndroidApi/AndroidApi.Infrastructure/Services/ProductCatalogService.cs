using AndroidApi.Application.Services;
using CraftedSpecially.AndroidApi.Domain;
using CraftedSpecially.AndroidApi.Infrastructure.Agents;

namespace AndroidApi.Infrastructure.Services
{
    internal class ProductCatalogService : IProductCatalogService
    {
        private readonly ICatalogusApi _catalogusApi;

        public ProductCatalogService(ICatalogusApi catalogusApi)
        {
            _catalogusApi = catalogusApi;
        }

        public async Task<IEnumerable<ProductCatalogItem>> GetCatalogItems()
        {
            var x = await _catalogusApi.GetCatalogusItems();

            return x.Select(x => new ProductCatalogItem
            {
            });
        }
    }
}
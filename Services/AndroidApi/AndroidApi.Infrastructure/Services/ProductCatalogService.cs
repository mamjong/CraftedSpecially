using AndroidApi.Application.Services;
using AndroidApi.Infrastructure.DTOs;
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

        public async Task CreateProductCatalogItem(string name, string description)
        {
            var command = new RegisterProductCommand(
                name: name,
                description: description
            );

            await _catalogusApi.RegisterNewProduct(command);
        }
    }
}
using CraftedSpecially.Catalog.Application.Interfaces;
using CraftedSpecially.Catalog.Domain.Aggregates.ProductAggregate;
using Microsoft.EntityFrameworkCore;

namespace CraftedSpecially.Catalog.Infrastructure.Persistence.EFCore
{
    public class EFProductRepository : IProductRepository
    {
        private readonly CatalogDbContext _dbContext;

        public EFProductRepository(CatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async ValueTask AddAsync(Product product)
        {
            var productDto = new ProductDto();

            await _dbContext.Products.AddAsync(productDto);
            await _dbContext.SaveChangesAsync();
        }

        public async ValueTask<bool> IsExistingProductAsync(string productName)
        {
            return await _dbContext.Products.AnyAsync(p => p.Name == productName);
        }
    }
}
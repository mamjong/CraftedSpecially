using Refit;

namespace CraftedSpeciallyLoadGenerator.IAndroidApi;

public interface IAndroidApi
{
    [Post("/Catalog")]
    public Task<IApiResponse> CreateNewProductAsync(CreateProductForm form);
}
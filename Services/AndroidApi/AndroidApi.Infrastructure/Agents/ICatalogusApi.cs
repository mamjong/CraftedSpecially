using AndroidApi.Infrastructure.DTOs;
using Refit;

namespace CraftedSpecially.AndroidApi.Infrastructure.Agents;

internal interface ICatalogusApi
{
    [Post("/productmanagement/command/registerproduct")]
    Task RegisterNewProduct();
}
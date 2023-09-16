using AndroidApi.DTOs;
using CraftedSpecially.AndroidApi.Application.Features.SeeProductCatalog;
using Microsoft.AspNetCore.Mvc;

namespace CraftedSpecially.AndroidApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogController : ControllerBase
{
    private readonly ILogger<CatalogController> _logger;
    private readonly SeeProductCatalog seeProductCatalog;

    public CatalogController(ILogger<CatalogController> logger, SeeProductCatalog seeProductCatalog)
    {
        _logger = logger;
        this.seeProductCatalog = seeProductCatalog;
    }

    [HttpGet]
    public async Task<IEnumerable<CatalogProduct>> Get()
    {
        var x = await seeProductCatalog.GetCatalogProduct();

        var dtoResult = x.Select(x => new CatalogProduct
        {
        });

        return dtoResult;
    }
}
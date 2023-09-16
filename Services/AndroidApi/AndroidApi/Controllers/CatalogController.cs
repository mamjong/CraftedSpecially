using CraftedSpecially.AndroidApi.Application.Features.SeeProductCatalog;
using Microsoft.AspNetCore.Mvc;

namespace CraftedSpecially.AndroidApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogController : ControllerBase
{
    private readonly ILogger<CatalogController> _logger;
    private readonly RegisterCatalogProduct registerCatalogProduct;

    public CatalogController(ILogger<CatalogController> logger, RegisterCatalogProduct registerCatalogProduct)
    {
        _logger = logger;
        this.registerCatalogProduct = registerCatalogProduct;
    }

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        await registerCatalogProduct.SendRegistration();

        return Ok();
    }
}
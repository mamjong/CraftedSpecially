public interface ICatalogusApi
{
    [Get("/catalogus")]
    Task<IEnumerable<CatalogusItem>> GetCatalogusItems();
}
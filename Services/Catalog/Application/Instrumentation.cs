using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace CraftedSpecially.Catalog.Application;

public sealed class Instrumentation : IDisposable
{
    public const string ActivitySourceName = "Catalog";
    public const string MeterName = "Catalog";
    
    private readonly Meter _meter;
    
    public Instrumentation()
    {
        var version = typeof(Instrumentation).Assembly.GetName().Version?.ToString() ?? "Unknown";
        ActivitySource = new ActivitySource(ActivitySourceName, version);
        _meter = new Meter(MeterName, version);
        CatalogRegisterRequestCounter = _meter.CreateCounter<long>("catalog_register_requests");
    }

    public ActivitySource ActivitySource { get; }
    
    public Counter<long> CatalogRegisterRequestCounter { get; }

    public void Dispose()
    {
        _meter.Dispose();
        ActivitySource.Dispose();
    }
}
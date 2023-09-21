using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace WebApi;

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
    }

    public ActivitySource ActivitySource { get; }

    public void Dispose()
    {
        _meter.Dispose();
        ActivitySource.Dispose();
    }
}
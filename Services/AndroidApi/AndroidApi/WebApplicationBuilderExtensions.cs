using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace CraftedSpecially.AndroidApi;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddOpenTelemetry(this WebApplicationBuilder builder)
    {
        // Build a resource configuration action to set service information.
        Action<ResourceBuilder> configureResource = resourceBuilder => resourceBuilder.AddService(
            serviceName: builder.Configuration.GetValue("ServiceName", defaultValue: "Catalog")!,
            serviceVersion: typeof(Program).Assembly.GetName().Version?.ToString() ?? "Unknown",
            serviceInstanceId: Environment.MachineName);

        // Build a configuration action to set the OTLP exporter endpoint.
        Action<OtlpExporterOptions> configureOtlpExporter = options =>
            options.Endpoint =
                new Uri(builder.Configuration.GetValue("Otlp:Endpoint", defaultValue: "http://localhost:4317")!);

        return builder
            .AddTracingAndMetrics(configureResource, configureOtlpExporter)
            .AddLogging(configureResource, configureOtlpExporter);
    }

    private static WebApplicationBuilder AddTracingAndMetrics(
        this WebApplicationBuilder builder,
        Action<ResourceBuilder> configureResource,
        Action<OtlpExporterOptions> configureOtlpExporter)
    {
        const string activitySourceName = "AndroidApi";
        const string meterName = "AndroidApi";
        
        builder.Services
            .AddOpenTelemetry()
            .ConfigureResource(configureResource)
            .WithTracing(tracingBuilder =>
            {
                // Use IConfiguration binding for AspNetCore instrumentation options.
                builder.Services.Configure<AspNetCoreInstrumentationOptions>(
                    builder.Configuration.GetSection("AspNetCoreInstrumentation"));

                tracingBuilder
                    .AddSource(activitySourceName)
                    .SetSampler<AlwaysOnSampler>()
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter(configureOtlpExporter);
            })
            .WithMetrics(metricsBuilder =>
            {
                metricsBuilder
                    .AddMeter(meterName)
                    .AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter(configureOtlpExporter);
            });

        return builder;
    }

    private static WebApplicationBuilder AddLogging(
        this WebApplicationBuilder builder,
        Action<ResourceBuilder> configureResource,
        Action<OtlpExporterOptions> configureOtlpExporter)
    {
        // Clear default logging providers used by WebApplication host and configure OpenTelemetry logging.
        builder.Logging
            .ClearProviders()
            .AddOpenTelemetry(options =>
            {
                // Note: See appsettings.json Logging:OpenTelemetry section for configuration.

                var resourceBuilder = ResourceBuilder.CreateDefault();
                configureResource(resourceBuilder);
                options
                    .SetResourceBuilder(resourceBuilder)
                    .AddOtlpExporter(configureOtlpExporter)
                    .AddConsoleExporter();
            });

        return builder;
    }
}
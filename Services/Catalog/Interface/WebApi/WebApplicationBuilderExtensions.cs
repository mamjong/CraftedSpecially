using CraftedSpecially.Catalog.Application;
using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace CraftedSpecially.Catalog.Interface.WebApi;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddOpenTelemetry(
        this WebApplicationBuilder builder)
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

        // Register the instrumentation service to expose ActivitySource for manual instrumentation.
        builder.Services.AddSingleton<Instrumentation>();

        return builder
            .AddTracingAndMetrics(configureResource, configureOtlpExporter)
            .AddLogging(configureResource, configureOtlpExporter);
    }

    private static WebApplicationBuilder AddTracingAndMetrics(
        this WebApplicationBuilder builder,
        Action<ResourceBuilder> configureResource,
        Action<OtlpExporterOptions> configureOtlpExporter)
    {
        builder.Services
            .AddOpenTelemetry()
            .ConfigureResource(configureResource)
            .WithTracing(tracingBuilder =>
            {
                // Use IConfiguration binding for AspNetCore instrumentation options.
                builder.Services.Configure<AspNetCoreInstrumentationOptions>(
                    builder.Configuration.GetSection("AspNetCoreInstrumentation"));

                tracingBuilder
                    .AddSource(Instrumentation.ActivitySourceName)
                    .SetSampler<AlwaysOnSampler>()
                    .AddAspNetCoreInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddOtlpExporter(configureOtlpExporter);
            })
            .WithMetrics(metricsBuilder =>
            {
                metricsBuilder
                    .AddMeter(Instrumentation.MeterName)
                    .AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation()
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
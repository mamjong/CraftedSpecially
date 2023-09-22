using CraftedSpecially.Catalog.Interface.WebApi;
using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Configure OpenTelemetry

// Build a resource configuration action to set service information.
Action<ResourceBuilder> configureResource = resourceBuilder => resourceBuilder.AddService(
    serviceName: builder.Configuration.GetValue("ServiceName", defaultValue: "Catalog")!,
    serviceVersion: typeof(Program).Assembly.GetName().Version?.ToString() ?? "Unknown",
    serviceInstanceId: Environment.MachineName);

// Build a configuration action to set the OTLP exporter endpoint.
Action<OtlpExporterOptions> configureOtlpExporter = options =>
    options.Endpoint = new Uri(builder.Configuration.GetValue("Otlp:Endpoint", defaultValue: "http://localhost:4317")!);

// Register the instrumentation service to expose ActivitySource for manual instrumentation.
builder.Services.AddSingleton<Instrumentation>();

// Configure tracing and metrics.
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

// Clear default logging providers used by WebApplication host.
builder.Logging.ClearProviders();

// Configure OpenTelemetry Logging.
builder.Logging.AddOpenTelemetry(options =>
{
    // Note: See appsettings.json Logging:OpenTelemetry section for configuration.

    var resourceBuilder = ResourceBuilder.CreateDefault();
    configureResource(resourceBuilder);
    options
        .SetResourceBuilder(resourceBuilder)
        .AddOtlpExporter(configureOtlpExporter)
        .AddConsoleExporter();
});

#endregion

builder.Services.AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

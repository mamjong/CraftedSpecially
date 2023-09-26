using CraftedSpecially.Catalog.Infrastructure.Persistence.EFCore;
using CraftedSpecially.Catalog.Interface.WebApi;
using CraftedSpecially.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddInfrastructure();

// Add OpenTelemetry tracing, metrics & logging.
builder.AddOpenTelemetry();

var app = builder.Build();

var serviceScope = app.Services.CreateScope();

using (var scope = serviceScope)
{
    var handler = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
    handler.Database.Migrate();
    handler.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
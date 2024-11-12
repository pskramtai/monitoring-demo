using MonitoringDemo.Endpoints;
using MonitoringDemo.Extensions;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.RegisterServices();

builder.AddOpenTelemetryExporters();

builder.Logging.AddOpenTelemetry(x =>
{
    x.IncludeScopes = true;
    x.IncludeFormattedMessage = true;
});

builder.Services.AddOpenTelemetry()
    .WithMetrics(x =>
    {
        x.AddRuntimeInstrumentation()
            .AddMeter(
                "Microsoft.AspNetCore.Hosting",
                "Microsoft.AspNetCore.Server.Kestrel",
                "System.Net.Http",
                "WeatherApp"
            ).AddPrometheusExporter();
    })
    .WithTracing(x =>
    {
        x.SetSampler(new AlwaysOnSampler());
        
        x.AddAspNetCoreInstrumentation();
        // .AddHttpClientInstrumentation();
    });

builder.Services.AddMetrics();

var app = builder.Build();

app.MapPrometheusScrapingEndpoint();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapWeatherForecastEndpoints();

app.Run();


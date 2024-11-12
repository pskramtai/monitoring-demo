using MonitoringDemo.Services;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace MonitoringDemo.Extensions;

public static class StartupExtensions
{
    public static IHostApplicationBuilder AddOpenTelemetryExporters(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<OpenTelemetryLoggerOptions>(logging => logging.AddOtlpExporter());
        builder.Services.ConfigureOpenTelemetryMeterProvider(metrics => metrics.AddOtlpExporter());
        builder.Services.ConfigureOpenTelemetryTracerProvider(tracing => tracing.AddOtlpExporter());

        builder.Services.AddOpenTelemetry().WithMetrics(x => x.AddPrometheusExporter());
        
        return builder;
    }

    public static IHostApplicationBuilder RegisterServices(this IHostApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<IWeatherService, WeatherService>()
            .AddScoped<IObservableWeatherService, ObservableWeatherService>();

        return builder;
    }
}
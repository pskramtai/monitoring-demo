using Microsoft.AspNetCore.Mvc;
using MonitoringDemo.Contracts;
using MonitoringDemo.Services;

namespace MonitoringDemo.Endpoints;

public static class WeatherEndpointConfiguration
{
    public static void MapWeatherForecastEndpoints(this WebApplication app)
    {
        app.MapGet();
        app.MapPut();
        app.MapPost();
    }
    
    private static void MapGet(this WebApplication app) =>
        app.MapGet("/weatherforecast", Handler)
            .AddEndpointFilter<EndpointFilter>()
            .WithName("GetWeatherForecast")
            .WithOpenApi();
    
    private static void MapPost(this WebApplication app) =>
        app.MapPost("/weatherforecast", Handler)
            .AddEndpointFilter<EndpointFilter>()
            .WithName("PostWeatherForecast")
            .WithOpenApi();
    
    private static void MapPut(this WebApplication app) =>
        app.MapPut("/weatherforecast", Handler)
            .AddEndpointFilter<EndpointFilter>()
            .WithName("PutWeatherForecast")
            .WithOpenApi();

    private static async Task<IEnumerable<WeatherForecast>> Handler([FromServices] IObservableWeatherService service) =>
        await service.GetForecast();
}
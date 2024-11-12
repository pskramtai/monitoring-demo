using MonitoringDemo.Contracts;

namespace MonitoringDemo.Services;

public interface IWeatherService
{
    Task<IEnumerable<WeatherForecast>> GetForecast();
}

public class WeatherService : IWeatherService
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];
    
    public async Task<IEnumerable<WeatherForecast>> GetForecast()
    {
        if (Random.Shared.Next(1, 100) <= 10)
        {
            throw new Exception();
        }
        
        var forecasts = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    Summaries[Random.Shared.Next(Summaries.Length)]
                ))
            .ToList();
        
        await Task.Delay(Random.Shared.Next(5, 1000));
        
        return forecasts;
    }
}
using System.Diagnostics;
using System.Diagnostics.Metrics;
using MonitoringDemo.Contracts;

namespace MonitoringDemo.Services;

public interface IObservableWeatherService : IWeatherService;

public class ObservableWeatherService : IObservableWeatherService
{
    private readonly IWeatherService _weatherService;
    
    private readonly Counter<int> _requestCounter;
    private readonly Histogram<double> _requestDurationHistogram;
    private readonly Gauge<int> _currentTemperatureGauge;

    public ObservableWeatherService(IMeterFactory meterFactory, IWeatherService service)
    {
        _weatherService = service;

        var meter = meterFactory.Create("WeatherApp");
        
        _requestCounter = meter.CreateCounter<int>("weather_forecast_requests", description: "The total number of weather forecast requests");
        
        var instrumentAdvice = new InstrumentAdvice<double> { HistogramBucketBoundaries = [10, 50, 100, 200, 500, 1000] };
        
        _requestDurationHistogram = meter.CreateHistogram(
            "weather_forecast_duration_seconds",
            unit: default,
            description: "Histogram of weather forecast request durations", 
            tags: default,
            instrumentAdvice
        );
        
        _currentTemperatureGauge = meter.CreateGauge<int>("weather_current_temperature", description: "The current temperature");
    }
    
    public async Task<IEnumerable<WeatherForecast>> GetForecast()
    {
        var sw = Stopwatch.StartNew();
        
        try
        { 
            var forecasts = (await _weatherService.GetForecast()).ToList();
            _currentTemperatureGauge.Record(forecasts.First().TemperatureC);

            return forecasts;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            _requestCounter.Add(1);
            _requestDurationHistogram.Record(sw.ElapsedMilliseconds);
        }
    }
}
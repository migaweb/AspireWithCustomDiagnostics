namespace HttpClientTest.Web;

public class WeatherApiClient(HttpClient httpClient)
{
    public async Task<WeatherForecast[]> GetWeatherAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<WeatherForecast>? forecasts = null;

        await foreach (var forecast in httpClient.GetFromJsonAsAsyncEnumerable<WeatherForecast>("/weatherforecast", cancellationToken))
        {
            if (forecasts?.Count >= maxItems)
            {
                break;
            }
            if (forecast is not null)
            {
                forecasts ??= [];
                forecasts.Add(forecast);
            }
        }

        return forecasts?.ToArray() ?? [];
    }
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

/*
 
public class WeatherForecast(DateOnly date, int temperatureC, string? summary)
{
  public DateOnly Date { get; set; } = date;
  public int TemperatureC { get; set; } = temperatureC;
  public string? Summary { get; set; } = summary;
  public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

*/

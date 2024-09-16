using HttpClientTest.ServiceDefaults;
using System.Text.Json;

namespace HttpClientTest.Web;

public static class WeatherApiClientBad
{
  public static async Task<WeatherForecast[]> GetWeatherAsync(CancellationToken cancellationToken = default)
  {
    using HttpClient client = new();
    client.BaseAddress = new("https://localhost:7552");
    client.DefaultRequestHeaders.Add("Type", "Bad");

    var result = await client.GetFromJsonAsync<List<WeatherForecast>>("/weatherforecast", cancellationToken);

    DiagnosticsConfig.ClickCounter.Add(1, new KeyValuePair<string, object?>("click.count", nameof(WeatherApiClientBad)));

    return result?.ToArray() ?? [];

  }
}

public class WeatherApiClientBetter
{
  private static readonly HttpClient _client = new();

  private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

  public async Task<WeatherForecast[]> GetWeatherAsync(CancellationToken cancellationToken = default)
  {
    var httpRequestMessage = new HttpRequestMessage
    {
      Method = HttpMethod.Get,
      RequestUri = new Uri("https://localhost:7552/weatherforecast")
    };
    httpRequestMessage.Headers.Add("Accept", "application/json");

    httpRequestMessage.Headers.Add("Type", "Better");

    var result = await _client.SendAsync(httpRequestMessage, cancellationToken);

    DiagnosticsConfig.ClickCounter.Add(1, new KeyValuePair<string, object?>("click.count", nameof(WeatherApiClientBetter)));

    if (result.IsSuccessStatusCode)
    {
      var jsonContent = await result.Content.ReadAsStringAsync(cancellationToken);
      var forecasts = JsonSerializer.Deserialize<List<WeatherForecast>>(jsonContent, _jsonSerializerOptions);

      return forecasts?.ToArray() ?? [];
    }

    return [];
  }
}

using System.Diagnostics.Metrics;

namespace HttpClientTest.ServiceDefaults;
public static class DiagnosticsConfig
{
  public const string ServiceName = "Weather";

  public static Meter Meter = new(ServiceName);

  public static Counter<int> ClickCounter = Meter.CreateCounter<int>("http.weather");
}

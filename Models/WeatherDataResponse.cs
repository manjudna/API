namespace OpenWeatherMapApi
{
    public class WeatherDataResponse
    {
        public bool IsSuccess { get; set; }

        public WeatherData weatherData { get; set; }
    }
}

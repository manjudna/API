using System.Threading.Tasks;

namespace OpenWeatherMapApi.Services
{
    public interface IWeatherRepository
    {
        Task<WeatherData> GetWeatherData(string city);
    }
}
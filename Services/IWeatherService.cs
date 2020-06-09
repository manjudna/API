using System.Threading.Tasks;

namespace OpenWeatherMapApi.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWeatherService
    {
        Task<WeatherDataResponse> GetWeatherData(string city);
    }
}
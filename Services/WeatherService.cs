using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenWeatherMapApi.Services
{

    /// <summary>
    /// 
    /// </summary>
    public class WeatherService : IWeatherService
    {
               
        private readonly IWeatherRepository _weatherRepository;
        private readonly IWeatherValidation _weatherValidation;
        public WeatherService(IWeatherRepository weatherRepository,IWeatherValidation weatherValidation)
        {
            _weatherRepository = weatherRepository;
            _weatherValidation = weatherValidation;
        }

        /// <summary>
        /// GetWeatherData
        /// </summary>
        /// <returns>WeatheData</returns>
        public async Task<WeatherDataResponse> GetWeatherData(string city)
        {
            var weatherDataResponse = new WeatherDataResponse();

            if (!_weatherValidation.IsValidCity(city))
            {
                weatherDataResponse.IsSuccess = false;
                weatherDataResponse.weatherData = null;
                return weatherDataResponse;
            }

            var weatherForecast = await _weatherRepository.GetWeatherData(city);
            weatherDataResponse.weatherData = weatherForecast;
            weatherDataResponse.IsSuccess = true;

            return weatherDataResponse;
          }

    }
}

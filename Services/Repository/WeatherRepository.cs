using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace OpenWeatherMapApi.Services
{

    public class WeatherRepository : IWeatherRepository
    {
        private readonly IMemoryCache _memoryCache;
        private AppSettings AppSettings { get; set; }

        public WeatherRepository(IMemoryCache memoryCache, IOptions<AppSettings> settings)
        {
            _memoryCache = memoryCache;
            AppSettings = settings.Value;
        }
        public async Task<WeatherData> GetWeatherData(string city)
        {

            string cacheKey = city.ToLower();
           
            if (!_memoryCache.TryGetValue(cacheKey, out WeatherData weatherForecast))
            {
                string apiURL = AppSettings.OpenWeatherApiURL.Replace("{city}",city);
                var client = new RestClient(apiURL);
                var request = new RestRequest(Method.GET);
                IRestResponse response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    // Deserialize the string content into JToken object
                    var content = JsonConvert.DeserializeObject<JToken>(response.Content);
                    var weatherData = content.ToObject<WeatherResponse>();
                    if (weatherForecast == null)
                        weatherForecast = new WeatherData();
                 
                    weatherForecast = WeatherMapper.MapWeatherObjects(weatherData, weatherForecast);
                     //add redis cache here ideally

                    //TODO::to be moved to Seperate class to make use of memory related operations 
                    var cacheExpiryOption = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddHours(6),
                        Priority = CacheItemPriority.Normal,
                        SlidingExpiration = TimeSpan.FromMinutes(5)
                    };
                    _memoryCache.Set(cacheKey, weatherForecast, cacheExpiryOption);

                }

            }

            return weatherForecast;

        }
    }
}
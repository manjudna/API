using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using RestSharp;
using System;
using System.Net.Http;
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
                var noofret = 0;
                var policy = Policy.
                    Handle<Exception>().
                    OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                    .RetryAsync(3, (ex, retryCount) =>
                    {
                        noofret = retryCount;
                    });

                string apiURL = AppSettings.OpenWeatherApiURL.Replace("{city}", city);
                //var client = new RestClient(apiURL);
                //var request = new RestRequest(Method.GET);
                //IRestResponse response =  await client.ExecuteAsync(request);


                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await policy.ExecuteAsync(
                    () => httpClient.GetAsync(apiURL));
                if (response.IsSuccessStatusCode)
                {
                    var resultsfromapi = response.Content.ReadAsStringAsync();
                    var content = JsonConvert.DeserializeObject<JToken>(resultsfromapi.Result);
                    var weatherData1 = content.ToObject<WeatherResponse>();
                    if (weatherForecast == null)
                        weatherForecast = new WeatherData();

                    weatherForecast = WeatherMapper.MapWeatherObjects(weatherData1, weatherForecast);
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


                //if (response.IsSuccessful)
                //{
                //    // Deserialize the string content into JToken object
                //    var content = JsonConvert.DeserializeObject<JToken>(response.Content);
                //    var weatherData = content.ToObject<WeatherResponse>();
                //    if (weatherForecast == null)
                //        weatherForecast = new WeatherData();

                //    weatherForecast = WeatherMapper.MapWeatherObjects(weatherData, weatherForecast);
                //     //add redis cache here ideally

                //    //TODO::to be moved to Seperate class to make use of memory related operations 
                //    var cacheExpiryOption = new MemoryCacheEntryOptions
                //    {
                //        AbsoluteExpiration = DateTime.Now.AddHours(6),
                //        Priority = CacheItemPriority.Normal,
                //        SlidingExpiration = TimeSpan.FromMinutes(5)
                //    };
                //    _memoryCache.Set(cacheKey, weatherForecast, cacheExpiryOption);

                //}

            }

            return weatherForecast;

        }
    }
}
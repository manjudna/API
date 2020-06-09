using System;

namespace OpenWeatherMapApi
{
    /// <summary>
    /// 
    /// </summary>
    public class WeatherData
    {
        public int TemperatureC { get; set; }

        public int TemperatureMax { get; set; }

        public int TemperatureMin { get; set; }

        public int Pressure { get; set; }

        public int Humidity { get; set; }

        public int Sunrise { get; set; }

        public int Sunset { get; set; }
        public string LocationName { get; set; }

    }
}

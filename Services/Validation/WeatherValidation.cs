using System;
using System.Text.RegularExpressions;

namespace OpenWeatherMapApi.Services
{
    public class WeatherValidation : IWeatherValidation
    {
        public bool IsValidCity(string city)
        {
            return Regex.Match(city, @"^([a-zA-Z]+|[a-zA-Z]+\s[a-zA-Z]+)$").Success;
        }
    }
}

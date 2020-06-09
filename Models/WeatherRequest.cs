namespace OpenWeatherMapApi
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Not used this, as I am not using POST HTTP for performing get operation, to keep it simple
    /// </summary>
    public class WeatherRequest
        {
            [Required]
            [MaxLength(30)]
            public string CityName { get; set; }
        }
}

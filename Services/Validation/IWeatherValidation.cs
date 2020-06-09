namespace OpenWeatherMapApi.Services
{
    public interface IWeatherValidation
    {
        bool IsValidCity(string city);
    }
}

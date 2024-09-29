namespace CoffeeMachine.ExternalServices
{
    public interface IOpenWeather
    {
        Task<decimal> GetCityTemperatuerFromCache();
    }
}

using CoffeeMachine.AppLogic.CustomExceptions;
using CoffeeMachine.DataAccess.Entites;
using CoffeeMachine.DataAccess.Repositories;
using CoffeeMachine.ExternalServices;
using CoffeeMachine.Helpers;
using System;

namespace CoffeeMachine.AppLogic
{
    public class CoffeeStockLogic : ICoffeeStockLogic
    {
        private readonly ICoffeeStockRepository _coffeeStockRepository;
        private readonly ITimeProviderByTimeZone _timeProvider;
        private readonly IOpenWeather _openWeather;

        public CoffeeStockLogic(ICoffeeStockRepository coffeeStockRepository, 
            ITimeProviderByTimeZone timeProvider, IOpenWeather openWeather)
        {
            _coffeeStockRepository = coffeeStockRepository ?? throw new ArgumentNullException(nameof(coffeeStockRepository));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
            _openWeather = openWeather ?? throw new ArgumentNullException(nameof(openWeather));
        }

        public async Task<CoffeeDto> GetCoffeeAsync()
        {
            var Now = _timeProvider.GetDateTimeOffsetNow();
            if (Now.Date.Month == 4 && Now.Date.Day == 1)
            {
                throw new TeaPotException(Consts.TeaPotExceptionMessage);
            }

            var result = await GetCoffeeStockAsync();

            if (result != null && result.Quantity > 0)
            {
                result.Quantity = result.Quantity - 1;
                UpdateStock(result);

                return new CoffeeDto(Consts.HotCoffee, Now.ToString("yyyy-MM-ddTHH:mm:sszzz"));
            }
            throw new ServiceUnavailableException(Consts.ServiceUnavailableMessage);
        }

        public async Task<CoffeeDto> GetCoffeeAsyncV2()
        {
            var coffee = await GetCoffeeAsync();
            var todayTemperature = await _openWeather.GetCityTemperatuerFromCache();

            if (todayTemperature > 30)
            {
                return new CoffeeDto(Consts.ColdCoffee, coffee.prepared);
            }

            return coffee;
        }
        public async Task RefillStock()
        {
            var coffeeStock = await GetCoffeeStockAsync();
            coffeeStock.Quantity = 4;
            coffeeStock.RefillDate = _timeProvider.GetDateTimeOffsetNow();
            UpdateStock(coffeeStock);
        }

        private void UpdateStock(CoffeeStock coffeeStock)
        {
            _coffeeStockRepository.UpdateStock(coffeeStock);
        }

        private async Task<CoffeeStock> GetCoffeeStockAsync()
        {
            return await _coffeeStockRepository.GetCoffeeStockAsync().ConfigureAwait(false);
        }
    }
}


using CoffeeMachine.AppLogic.CustomExceptions;
using CoffeeMachine.DataAccess.Entites;
using CoffeeMachine.DataAccess.Repositories;
using System;

namespace CoffeeMachine.AppLogic
{
    public class CoffeeStockLogic : ICoffeeStockLogic
    {
        private readonly ICoffeeStockRepository _coffeeStockRepository;
        private readonly ITimeProviderByTimeZone _timeProvider;

        public CoffeeStockLogic(ICoffeeStockRepository coffeeStockRepository, ITimeProviderByTimeZone timeProvider)
        {
            _coffeeStockRepository = coffeeStockRepository ?? throw new ArgumentNullException(nameof(coffeeStockRepository));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
        }

        public async Task<CoffeeDto> GetCoffeeAsync()
        {
            var Now = _timeProvider.GetDateTimeOffsetNow();
            if (Now.Date.Month == 4 && Now.Date.Day == 1)
            {
                throw new TeaPotException("I’m a teapot");
            }

            var result = await GetCoffeeStockAsync();

            if (result != null && result.Quantity > 0)
            {
                result.Quantity = result.Quantity - 1;
                UpdateStock(result);

                return new CoffeeDto("Your piping hot coffee is ready", Now.ToString("yyyy-MM-ddTHH:mm:sszzz"));
            }
            throw new ServiceUnavailableException("no coffee");
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

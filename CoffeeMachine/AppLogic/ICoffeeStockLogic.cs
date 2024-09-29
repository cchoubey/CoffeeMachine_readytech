using CoffeeMachine.DataAccess.Entites;

namespace CoffeeMachine.AppLogic
{
    public interface ICoffeeStockLogic
    {
        Task<CoffeeDto> GetCoffeeAsync();
        Task RefillStock();
        Task<CoffeeDto> GetCoffeeAsyncV2();
    }
}

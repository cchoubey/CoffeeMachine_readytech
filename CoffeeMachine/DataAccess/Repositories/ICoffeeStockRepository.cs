using CoffeeMachine.DataAccess.Entites;

namespace CoffeeMachine.DataAccess.Repositories
{
    public interface ICoffeeStockRepository
    {
        Task<CoffeeStock> GetCoffeeStockAsync();
        void UpdateStock(CoffeeStock coffeeStock);
    }
}

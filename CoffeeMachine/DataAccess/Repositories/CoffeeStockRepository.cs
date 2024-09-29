using CoffeeMachine.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMachine.DataAccess.Repositories
{
    public class CoffeeStockRepository : ICoffeeStockRepository
    {
        private readonly AppDbContext _appDbContext;

        public CoffeeStockRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<CoffeeStock> GetCoffeeStockAsync()
        {
            var coffesstock = await _appDbContext.CoffeeStocks.FirstOrDefaultAsync().ConfigureAwait(false);
            return coffesstock;
        }

        public void UpdateStock(CoffeeStock coffeeStock)
        {
            _appDbContext.Attach(coffeeStock);
            _appDbContext.SaveChanges();
        }
    }
}

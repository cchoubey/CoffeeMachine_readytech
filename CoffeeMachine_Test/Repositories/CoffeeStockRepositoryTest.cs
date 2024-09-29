using CoffeeMachine.DataAccess;
using CoffeeMachine.DataAccess.Repositories;
using CoffeeMachine_Test.Helper;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMachine_Test.Repositories
{
    public class CoffeeStockRepositoryTest
    {
        private AppDbContext _context;
        public ICoffeeStockRepository IcoffeeStockRepository;
        public CoffeeStockRepositoryTest()
        {
            _context = new AppDbContext(TestDatabaseConnection.GetConnection());

            if (_context != null)
            {
                if (_context != null)
                {
                    _context.Database.EnsureDeleted();
                    _context.Database.EnsureCreated();
                }
            }
            IcoffeeStockRepository = new CoffeeStockRepository(_context);
        }

        [Fact]
        public async void GetCoffeeStockAsync()
        {
            //act
           var coffee =  await IcoffeeStockRepository.GetCoffeeStockAsync();

            //assert
           Assert.NotEmpty(_context.CoffeeStocks);
            coffee.Quantity.ShouldBe(4);
        }

        [Fact]
        public async void UpdateCoffeeStockAsync()
        {
            //arrange
            int newQuantity = 3;
            var coffee = await IcoffeeStockRepository.GetCoffeeStockAsync();
            
            coffee.Quantity = newQuantity;

            //act
            IcoffeeStockRepository.UpdateStock(coffee);

            var updated_coffee = await IcoffeeStockRepository.GetCoffeeStockAsync();

            //assert
            updated_coffee.Quantity.ShouldBe(newQuantity);
        }
    }
}

using CoffeeMachine.DataAccess;
using CoffeeMachine_Test.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMachine_Test
{
    public class TestDBConnection
    {
        [Fact]
        public void CheckConnection()
        {
            AppDbContext appDbContext = new AppDbContext(TestDatabaseConnection.GetConnection());

            if (appDbContext != null)
            {
                appDbContext.Database.EnsureDeleted();
                appDbContext.Database.EnsureCreated();
            }

            Assert.NotNull(appDbContext);
            Assert.NotEmpty(appDbContext.CoffeeStocks);
        }
    }
}

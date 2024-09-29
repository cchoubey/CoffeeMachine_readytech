using CoffeeMachine.DataAccess;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMachine_Test.Helper
{
    public class TestDatabaseConnection
    {
        public static DbContextOptions<AppDbContext> GetConnection()
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            DbContextOptions<AppDbContext>  _dbContextOptions = builder.UseSqlite(connection).Options;
            return _dbContextOptions;
        }
    }
}

using CoffeeMachine.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMachine.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<CoffeeStock> CoffeeStocks { get; set; } = null!;
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
         
            // we can extract seeding of relevant tables
            // to different class. But for the purpose of this work
            // leaving it here

            modelBuilder.Entity<CoffeeStock>().HasData(
                new CoffeeStock() 
                { 
                    Id = 1,
                    Quantity  = 4,
                    RefillDate = DateTimeOffset.Now
                });
        }
    }
}

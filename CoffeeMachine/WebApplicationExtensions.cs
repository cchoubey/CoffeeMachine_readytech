using CoffeeMachine.AppLogic;
using CoffeeMachine.DataAccess;
using CoffeeMachine.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMachine
{
    public static class WebApplicationExtensions
    {
        public static async Task InitializeDbAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Database.MigrateAsync();
        }

        public static IServiceCollection AddServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("ConString");
            services.AddSqlServer<AppDbContext>(connString)
                .AddScoped<ITimeProviderByTimeZone, TimeProviderByTimeZone>()
                .AddScoped<ICoffeeStockRepository, CoffeeStockRepository>()
                .AddScoped<ICoffeeStockLogic, CoffeeStockLogic>();
            return services;
        }
    }
}

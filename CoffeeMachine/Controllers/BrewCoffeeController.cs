using CoffeeMachine.AppLogic;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeMachine.Controllers
{
    [ApiController]
    [Route("brew-coffee")]
    public class BrewCoffeeController : ControllerBase
    {
        private readonly ILogger<BrewCoffeeController> _logger;
        private readonly ICoffeeStockLogic _coffeeStockLogic;
        public BrewCoffeeController(ICoffeeStockLogic coffeeStockLogic, ILogger<BrewCoffeeController> logger)
        {
            _logger = logger;
            _coffeeStockLogic = coffeeStockLogic;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var coffee = await _coffeeStockLogic.GetCoffeeAsync().ConfigureAwait(false);
            return Ok(coffee);
        }

        [HttpPut]
        public async Task<IActionResult> Put()
        {
            await _coffeeStockLogic.RefillStock();
            return Ok("success");
        }
    }
}

using Asp.Versioning;
using CoffeeMachine.AppLogic;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeMachine.Controllers
{
    [ApiVersion(1)]
    [ApiVersion(2)]
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

        [MapToApiVersion(1)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var coffee = await _coffeeStockLogic.GetCoffeeAsync().ConfigureAwait(false);
            return Ok(coffee);
        }

        [MapToApiVersion(2)]
        [HttpGet]
        public async Task<IActionResult> GetWithCityInfo()
        {
            _logger.LogInformation("v2 called");
            var coffee = await _coffeeStockLogic.GetCoffeeAsyncV2().ConfigureAwait(false);
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

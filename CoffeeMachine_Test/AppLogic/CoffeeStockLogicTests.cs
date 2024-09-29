using CoffeeMachine.AppLogic;
using CoffeeMachine.AppLogic.CustomExceptions;
using CoffeeMachine.DataAccess.Entites;
using CoffeeMachine.DataAccess.Repositories;
using CoffeeMachine_Test.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;
using Shouldly;

namespace CoffeeMachine_Test.AppLogic
{
    public class CoffeeStockLogicTests
    {

        [Fact]
        public void ConstructorThrowsArgumentNullException_When_CoffeeRepoNull()
        {
            //arrange
            var timeProvider = new Mock<ITimeProviderByTimeZone>();
            // act
            var action = () => new CoffeeStockLogic(null, timeProvider.Object);

            // assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConstructorThrowsArgumentNullException_When_TimeProviderNull()
        {
            //arrange
            var coffeeRepo = new Mock<ICoffeeStockRepository>();
            // act
            var action = () => new CoffeeStockLogic(coffeeRepo.Object, null);

            // assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ShouldThrowTeaPotException_on_FirstApril()
        {
            //arrange
            var dto = new DateTimeOffset(2024, 4, 1, 0, 0, 0, new TimeSpan());
            var timeProvider = new Mock<ITimeProviderByTimeZone>();
            timeProvider.Setup(l => l.GetDateTimeOffsetNow()).Returns(dto);
            var coffeeStockRepository = new Mock<ICoffeeStockRepository>();

            // act
            var action = () => new CoffeeStockLogic(coffeeStockRepository.Object, timeProvider.Object).GetCoffeeAsync();

            // assert
            action.ShouldThrow<TeaPotException>();
        }

        [Fact]
        public async void ShouldReturnCorrectOffSetForTimeZoneId()
        {
            // arrange
            var inMemorySettings = new Dictionary<string, string> {
                {"TimeZoneId", "India Standard Time"}};

            IConfiguration configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection(inMemorySettings)
                            .Build();

            ITimeProviderByTimeZone timeProvider = new TimeProviderByTimeZone(configuration);

            var coffeeStock = new CoffeeStock { Id = 1, Quantity = 4 };

            var coffeeStockRepository = new Mock<ICoffeeStockRepository>();

            coffeeStockRepository.Setup(l => l.GetCoffeeStockAsync()).Returns(Task.FromResult(coffeeStock));

            var coffeeStockLogic = new CoffeeStockLogic(coffeeStockRepository.Object, timeProvider);

            // act
            var result = await coffeeStockLogic.GetCoffeeAsync();

            var dto = DateTimeOffset.Parse(result.prepared);

            // assert
            result.message.ShouldBe("Your piping hot coffee is ready");
            dto.Offset.ShouldBe(new TimeSpan(5,30,0));
        }

        [Fact]
        public async void ShouldReturnCoffeeInStock()
        {
            // arrange
            var dto = DateTimeOffset.Now;
            var timeProvider = new Mock<ITimeProviderByTimeZone>();
            timeProvider.Setup(l => l.GetDateTimeOffsetNow()).Returns(dto);

            var coffeeStock = new CoffeeStock { Id = 1, Quantity = 4 };

            var coffeeStockRepository = new Mock<ICoffeeStockRepository>();

            coffeeStockRepository.Setup(l => l.GetCoffeeStockAsync()).Returns(Task.FromResult(coffeeStock));

            var coffeeStockLogic = new CoffeeStockLogic(coffeeStockRepository.Object, timeProvider.Object);

            // act
            var result = await coffeeStockLogic.GetCoffeeAsync();

            // assert
            result.message.ShouldBe("Your piping hot coffee is ready");
            result.prepared.ShouldBe(dto.ToString("yyyy-MM-ddTHH:mm:sszzz"));
        }

        [Fact]
        public async void ShouldReturnCoffeeInStock_And_ReduceStock()
        {
            // arrange
            var dto = DateTimeOffset.Now;
            var timeProvider = new Mock<ITimeProviderByTimeZone>();
            timeProvider.Setup(l => l.GetDateTimeOffsetNow()).Returns(dto);

            var coffeeStockRepository = new CoffeeStockRepositoryTest();

            var coffeeStockLogic = new CoffeeStockLogic(coffeeStockRepository.IcoffeeStockRepository, timeProvider.Object);

            // act
            var result = await coffeeStockLogic.GetCoffeeAsync();

            // assert
            result.message.ShouldBe("Your piping hot coffee is ready");
            result.prepared.ShouldBe(dto.ToString("yyyy-MM-ddTHH:mm:sszzz"));

            // act
            var reducedCoffeeStock = await coffeeStockRepository.IcoffeeStockRepository.GetCoffeeStockAsync();
            // assert
            reducedCoffeeStock.Quantity.ShouldBe(3);
        }

        [Fact]
        public void ShouldThrow_ServiceUnavailableException_When_CoffeeNotInStock()
        {
            //arrange
            var dto = DateTimeOffset.Now;
            var timeProvider = new Mock<ITimeProviderByTimeZone>();
            timeProvider.Setup(l => l.GetDateTimeOffsetNow()).Returns(dto);

            var coffeeStock = new CoffeeStock { Id = 1, Quantity = 0 };

            var coffeeStockRepository = new Mock<ICoffeeStockRepository>();

            coffeeStockRepository.Setup(l => l.GetCoffeeStockAsync()).Returns(Task.FromResult(coffeeStock));

            var coffeeStockLogic = new CoffeeStockLogic(coffeeStockRepository.Object, timeProvider.Object);

            // act
            var action = coffeeStockLogic.GetCoffeeAsync();

            // assert
            action.ShouldThrow<ServiceUnavailableException>();
        }
    }
}

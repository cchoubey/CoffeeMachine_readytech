using CoffeeMachine.AppLogic;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Net.Http.Json;

namespace CoffeeMachine_IntegrationTest
{
    public class BrewCoffeeControllerTests
    {
        [Fact]
        public async Task GetCoffee()
        {
            //Arrange
            var app = new CoffeeMachineWebApplicationFactory();

            var client = app.CreateClient();

            //Act
            var response = await client.GetAsync("brew-coffee");

            //Assert
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<CoffeeDto>();
            result.message.Should().Be("Your piping hot coffee is ready");
        }

        [Fact]
        public async Task ShouldRespondWithServiceUnavailableOn5thRequests()
        {
            //Arrange
            var app = new CoffeeMachineWebApplicationFactory();

            var client = app.CreateClient();

            //Act

            for (int i = 0; i < 5; i++)
            {
                var response = await client.GetAsync("brew-coffee");

                //Assert             

                if (i < 4)
                {
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadFromJsonAsync<CoffeeDto>();
                    result.message.Should().Be("Your piping hot coffee is ready");

                }
                else
                {
                    var body = await response.Content.ReadAsStringAsync();
                    response.IsSuccessStatusCode.Should().BeFalse();
                    response.StatusCode.Should().Be(System.Net.HttpStatusCode.ServiceUnavailable);
                    body.Should().BeEmpty();
                }
            }
        }

        [Fact]
        public async Task ShouldAccessV2Method()
        {
            //Arrange
            var app = new CoffeeMachineWebApplicationFactory();

            var client = app.CreateClient();
            client.DefaultRequestHeaders.Add("X-Api-Version", "2.0");
            //Act
            var response = await client.GetAsync("brew-coffee");

            //Assert
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<CoffeeDto>();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
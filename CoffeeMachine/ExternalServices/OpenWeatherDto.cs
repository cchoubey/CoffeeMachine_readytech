namespace CoffeeMachine.ExternalServices
{
    public class OpenWeatherDto
    {
        public string name { get; set; }
        public Main main { get; set; }
    }
    public class Main
    {
        public decimal temp { get; set; }
    }
    public class Coord
    {
        public decimal lon { get; set; }
        public decimal lat { get; set; }
    }
}

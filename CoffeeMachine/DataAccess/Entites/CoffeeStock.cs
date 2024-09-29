namespace CoffeeMachine.DataAccess.Entites
{
    public class CoffeeStock
    {
        public int Id { get; set; }
        public required int Quantity { get; set; }
        public DateTimeOffset RefillDate { get; set; }
    }
}

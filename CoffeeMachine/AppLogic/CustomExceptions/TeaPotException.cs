namespace CoffeeMachine.AppLogic.CustomExceptions
{
    public class TeaPotException : Exception
    {
        public int StatuCode { get; private set; }
        public TeaPotException(string message) : base(message)
        {
            StatuCode = StatusCodes.Status418ImATeapot;
        }
    }
}

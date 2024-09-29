using System.Net;

namespace CoffeeMachine.AppLogic.CustomExceptions
{
    public class ServiceUnavailableException : Exception
    {
        public int StatuCode { get; private set; }
        public ServiceUnavailableException(string message) : base(message)
        {
            StatuCode = (int)HttpStatusCode.ServiceUnavailable;
        }
    }
}

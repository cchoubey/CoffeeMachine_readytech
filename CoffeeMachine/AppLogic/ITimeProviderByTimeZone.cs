namespace CoffeeMachine.AppLogic
{
    public interface ITimeProviderByTimeZone
    {
        DateTimeOffset GetDateTimeOffsetNow();
    }
}

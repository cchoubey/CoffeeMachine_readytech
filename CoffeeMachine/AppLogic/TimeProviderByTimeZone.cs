
namespace CoffeeMachine.AppLogic
{
    public class TimeProviderByTimeZone : ITimeProviderByTimeZone
    {
        private readonly IConfiguration _config;
        public TimeProviderByTimeZone(IConfiguration config)
        {
            _config = config;
        }
        public DateTimeOffset GetDateTimeOffsetNow()
        {
            var tzi = TimeZoneInfo.FindSystemTimeZoneById(_config.GetValue<string>("TimeZoneId"));
            var dto = DateTimeOffset.Now;

            return dto.ToOffset(tzi.GetUtcOffset(dto));
        }
    }
}

using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace CoffeeMachine.ExternalServices
{
    public class OpenWeather : IOpenWeather
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<OpenWeather> _logger;
        private readonly IMemoryCache _memoryCache;
        private const string TemperatureCacheKey = "CurrentTemperature";
        public OpenWeather(ILogger<OpenWeather> logger, IConfiguration configuration,
            IMemoryCache memoryCache)
        {
            _configuration = configuration;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<decimal> GetCityTemperatuerFromCache()
        {
            if (_memoryCache.TryGetValue(TemperatureCacheKey, out OpenWeatherDto openWeatherDto))
            {
                _logger.LogInformation("reading from cache");
            }
            else
            {
                var expiryHours = _configuration.GetValue<int>("ExpireCacheInHours");
                openWeatherDto = await GetCityWeather();

                if (openWeatherDto != null)
                {
                    var memoryCacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromHours(expiryHours))
                        .SetPriority(CacheItemPriority.Normal);

                    _memoryCache.Set(TemperatureCacheKey, openWeatherDto, memoryCacheEntryOptions);
                }
            }

            return openWeatherDto?.main.temp ?? 0;
        }

        private async Task<OpenWeatherDto> GetCityWeather()
        {
            try
            {
                var OpenWeatherURL = _configuration.GetValue<string>("OpenWeatherURL");
                var city = _configuration.GetValue<string>("City:Name");

                var apiId = _configuration.GetValue<string>("OpenWeatherApiKey");
                var _httpClient = new HttpClient
                {
                    BaseAddress = new Uri(OpenWeatherURL)
                };
                var response = await _httpClient.GetAsync($"?q={city}&appid={apiId}&units=metric");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<OpenWeatherDto>(data);

                    return responseObject;
                }
                _logger.LogError("Open weather API returned {response}", response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
            }
            return null;
        }
    }
}

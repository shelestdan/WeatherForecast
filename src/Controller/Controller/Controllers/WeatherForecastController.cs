using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using rush01.WeatherClient.Services;
using rush01.WeatherClient.Models;
using Microsoft.AspNetCore.Http;

namespace Controller.Controllers
{
    [ApiController]
    [Route("weatherforecast")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly WeatherClient _weatherClient;
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private const string DefaultCitySessionKey = "DefaultCity";

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherClient weatherClient, IMemoryCache cache, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _weatherClient = weatherClient;
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("GetWeather")]
        [ProducesResponseType(typeof(WeatherResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<WeatherResponse>> GetWeather(string cityName = null)
        {
            string defaultCity = _cache.Get<string>(DefaultCitySessionKey);

            if (string.IsNullOrEmpty(cityName))
            {
                cityName = defaultCity;
            }

            if (!string.IsNullOrEmpty(cityName))
            {
                return await _weatherClient.GetWeatherByCity(cityName);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("GetWeatherByCoordinates")]
        [ProducesResponseType(typeof(WeatherResponse), 200)]
        public async Task<ActionResult<WeatherResponse>> GetWeatherByCoordinates(float latitude, float longitude)
        {
            return await _weatherClient.GetWeatherByCoordinates(latitude, longitude);
        }


        [HttpPost("SetDefaultCity")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult SetDefaultCity(string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                return BadRequest();
            }

            _cache.Set(DefaultCitySessionKey, city);
            return Ok();
        }

        [HttpGet("GetDefaultCity")]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult GetDefaultCity()
        {
            string defaultCity = _cache.Get<string>(DefaultCitySessionKey);
            return Ok(defaultCity);
        }
    }
}

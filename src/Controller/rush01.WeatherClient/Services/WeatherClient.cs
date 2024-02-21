
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using rush01.WeatherClient.Models;

namespace rush01.WeatherClient.Services
{
    public class WeatherClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private string _defaultCity;

        public string WeatherApiKey { get; }

        public WeatherClient(IOptions<ServiceSettings> options)
        {
            _apiKey = options.Value.ApiKey;
            _httpClient = new HttpClient();
        }

        public void SetDefaultCity(string city)
        {
            _defaultCity = city;
        }

        public string GetDefaultCity()
        {
            return _defaultCity;
        }

        public async Task<WeatherResponse> GetWeatherByCoordinates(float latitude, float longitude)
        {
            string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={_apiKey}";

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(jsonResponse);
                return weatherResponse;
            }
            else
            {
                return null;
            }
        }

        public async Task<WeatherResponse> GetWeatherByCity(string cityName)
        {
            string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={_apiKey}";

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(jsonResponse);
                return weatherResponse;
            }
            else
            {
                return null;
            }
        }
    }
}

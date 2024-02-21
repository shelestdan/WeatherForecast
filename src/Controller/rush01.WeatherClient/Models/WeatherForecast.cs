namespace rush01.WeatherClient.Models
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }

    public class WeatherResponse
    {
        public Wind Wind { get; set; }
        public WeatherDescription[] Weather { get; set; }
        public MainWeatherData Main { get; set; }
        public string Name { get; set; }

        public float TemperatureC => (float)(Main.Temp - 273.15);
    }


    public class Wind
    {
        public float Speed { get; set; }
    }

    public class WeatherDescription
    {
        public string Description { get; set; }
    }

    public class MainWeatherData
    {
        public float Temp { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
    }
}

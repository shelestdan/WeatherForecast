using rush01.WeatherClient.Models;
using rush01.WeatherClient.Services;

namespace Controller
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var env = builder.Environment;

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            if (env.IsDevelopment())
            {
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();
            }

            // Зарегистрировать ServiceSettings и передать его в WeatherClient
            builder.Services.Configure<ServiceSettings>(builder.Configuration.GetSection("OpenWeatherSettings"));
            builder.Services.AddSingleton<WeatherClient>();

            // Добавляем поддержку сеансов
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Время жизни сеанса (30 минут в данном случае)
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Регистрируем IHttpContextAccessor
            builder.Services.AddHttpContextAccessor();

            // Регистрируем IMemoryCache
            builder.Services.AddMemoryCache();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Добавляем промежуточное ПО для использования сеансов
            app.UseSession();

            app.MapControllers();

            app.Run();
        }
    }
}

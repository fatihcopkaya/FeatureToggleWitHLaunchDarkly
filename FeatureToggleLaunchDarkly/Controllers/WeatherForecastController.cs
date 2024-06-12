using FeatureToggleLaunchDarkly.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FeatureToggleLaunchDarkly.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IFeatureToggleService _featureToggleService;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;       

        public WeatherForecastController(IFeatureToggleService featureToggleService, ILogger<WeatherForecastController> logger)
        {
            _featureToggleService = featureToggleService;
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            // deneme amaçlý hardcoded ilerlendi
            bool IsUserStandart = _featureToggleService.IsFeatureEnabled("1", "Standart");
            if (IsUserStandart) 
            {
                return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
           .ToArray();
            }
            else
            {
                return null;
            }
           
        }
    }
}

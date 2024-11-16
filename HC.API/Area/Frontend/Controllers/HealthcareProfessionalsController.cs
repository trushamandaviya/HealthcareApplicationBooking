using Microsoft.AspNetCore.Mvc;

namespace HC.API.Frontend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthcareProfessionalsController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<HealthcareProfessionalsController> _logger;

        public HealthcareProfessionalsController(ILogger<HealthcareProfessionalsController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetHealthcareProfessionals")]
        public IEnumerable<WeatherForecast> GetHealthcareProfessionals()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}

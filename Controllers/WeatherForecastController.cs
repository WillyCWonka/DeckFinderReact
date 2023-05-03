/*using Microsoft.AspNetCore.Mvc;

namespace MTGDeckFinder.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<Card> Get()
    {
         return Enumerable.Range(1, 5).Select(index => new WeatherForecast
          {
              Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
              TemperatureC = Random.Shared.Next(-20, 55),
              Summary = Summaries[Random.Shared.Next(Summaries.Length)]
          })
          .ToArray();
        return Enumerable.Range(1, 5).Select(index => new Card
        {
            Name = "card1",
            Count = index
        });
    }
}*/

using Microsoft.AspNetCore.Mvc;

namespace MTGDeckFinder.Controllers;

[ApiController]
[Route("[controller]")]
public class DeckController : ControllerBase
{
    private readonly ILogger<DeckController> _logger;
    private readonly Database _database;

    public DeckController(ILogger<DeckController> logger, Database database)
    {
        _logger = logger;
        _database = database;
    }

    [HttpGet]
    public IEnumerable<Card> Get(string card1, string card2)
    {
        _logger.LogInformation($"card1:{card1} card2:{card2}");
        var result = _database.GetDeckAsync(card1, card2);

        result.Wait();
        return result.Result;
    }

    [HttpGet("autocomplete1")]
    public List<string> AutoComplete(string card1)
    {
        _logger.LogInformation($"autocomplete card1:{card1}");
        var result = _database.AutoComplete(card1);
        result.Wait();
        return result.Result;
    }

    [HttpGet("autocomplete2")]
    public List<string> AutoComplete(string card1, string card2)
    {
        _logger.LogInformation($"autocomplete card1:{card1} card2:{card2}");
        var result = _database.AutoComplete(card1, card2);
        result.Wait();
        return result.Result;
    }
}

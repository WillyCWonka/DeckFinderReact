namespace MTGDeckFinder;
using Neo4j.Driver;

public class Database : IDisposable
{
    private readonly IDriver _driver;

    public Database(string uri, string user, string password)
    {
        _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
    }

    public async Task<List<Card>> GetDeckAsync(string card1, string card2)
    {
        await using var session = _driver.AsyncSession(SessionConfigBuilder.ForDatabase("mtgdecks"));

        var result = await session.RunAsync(
            "MATCH (c1:Card {name:$card1})-[:IN_MAIN]->(d:Deck)<-[:IN_MAIN]-(c2:Card {name:$card2}) " +
            "RETURN id(d)",
            new { card1 = card1, card2 = card2 });

        var deckIds = await result.ToListAsync<long>(r => r[0].As<long>());

        if (deckIds.Count == 0)
        {
            return new List<Card>();
        }

        // get a random deck
        var deckId = deckIds[new Random().Next(deckIds.Count)];
        result = await session.RunAsync(
            "MATCH (c:Card)-[r:IN_MAIN]->(d:Deck) " +
            "WHERE id(d) = $deckId " +
            "RETURN c.name, r.count",
            new { deckId = deckId });

        var cards = await result.ToListAsync<Card>(r => new Card
        {
            Name = r[0].As<string>(),
            Count = r[1].As<int>()
        });

        return cards;
    }
    public async Task<List<string>> AutoComplete(string card1)
    {
        await using var session = _driver.AsyncSession(SessionConfigBuilder.ForDatabase("mtgdecks"));

        var result = await session.RunAsync(
        @"MATCH (c:Card)
        WHERE c.name=~$card1
        RETURN c.name",
            new { card1 = "(?i).*" + card1 + ".*" });

        var cards = await result.ToListAsync<string>(r => r[0].As<string>());

        return cards;
    }

    public async Task<List<string>> AutoComplete(string card1, string card2)
    {
        await using var session = _driver.AsyncSession(SessionConfigBuilder.ForDatabase("mtgdecks"));

        var result = await session.RunAsync(
        @"MATCH (c1:Card{name:$card1})-[:IN_MAIN]->(d:Deck)<-[:IN_MAIN]-(c2:Card)
        WHERE c2.name=~$card2
        RETURN DISTINCT c2.name",
            new { card1 = card1, card2 = "(?i).*" + card2 + ".*" });

        var cards = await result.ToListAsync<string>(r => r[0].As<string>());

        return cards;
    }

    public void Dispose()
    {
        _driver?.Dispose();
    }
}
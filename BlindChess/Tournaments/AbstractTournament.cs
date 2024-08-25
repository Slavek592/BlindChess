using BlindChessEngine;

namespace BlindChess.Tournaments;

public abstract class AbstractTournament
{
    protected readonly uint GamesPerPair;
    protected readonly Dictionary<string, (IGameStrategy GameStrategy, IBlindStrategy BlindStrategy)> Participants;
    protected readonly Dictionary<string, ulong> GameScores = new();
    protected readonly Dictionary<string, ulong> BlindScores = new();
    protected readonly Dictionary<string, ulong> TotalScores = new();
    
    protected AbstractTournament(Dictionary<string,
        (IGameStrategy gameStrategy, IBlindStrategy blindStrategy)> participants, uint gamesPerPair)
    {
        GamesPerPair = gamesPerPair;
        Participants = participants;
        foreach (var participant in participants.Keys)
        {
            GameScores[participant] = 0;
            BlindScores[participant] = 0;
            TotalScores[participant] = 0;
        }
    }

    protected void PlaySeries(List<(string gamePlayer, string blindPlayer)> pairs)
    {
        List<(IGameStrategy, IBlindStrategy)> strategies = new();
        for (byte i = 0; i < pairs.Count; i++)
            strategies.Add((Participants[pairs[i].gamePlayer].GameStrategy,
                Participants[pairs[i].blindPlayer].BlindStrategy));
        uint[] scores = SimulateGames(strategies, GamesPerPair);
        for (byte i = 0; i < scores.Length; i++)
        {
            GameScores[pairs[i].gamePlayer] += scores[i];
            BlindScores[pairs[i].blindPlayer] += scores[i];
            TotalScores[pairs[i].gamePlayer] += scores[i];
            TotalScores[pairs[i].blindPlayer] += scores[i];
        }
    }

    public abstract void Play();

    public void PrintResults()
    {
        Console.WriteLine("Results:");
        Console.WriteLine("  GameStrategy results:");
        foreach (var result in GameScores.OrderByDescending(x => x.Value))
            Console.WriteLine($"    {result.Key}: {result.Value}");
        Console.WriteLine("  BlindStrategy results:");
        foreach (var result in BlindScores.OrderByDescending(x => x.Value))
            Console.WriteLine($"    {result.Key}: {result.Value}");
        Console.WriteLine("  Participant results:");
        foreach (var result in TotalScores.OrderByDescending(x => x.Value))
            Console.WriteLine($"    {result.Key}: {result.Value}");
    }
    
    protected static uint[] SimulateGames(List<(IGameStrategy, IBlindStrategy)> strategies, uint games)
    {
        uint[] scores = new uint[strategies.Count];
        for (byte i = 0; i < strategies.Count; i++)
            scores[i] = 0;
        Dictionary<Player, (IGameStrategy, IBlindStrategy)> players = new();
        players[Player.White] = (strategies[0].Item1, strategies[0].Item2);
        players[Player.Black] = (strategies[1].Item1, strategies[1].Item2);
        for (uint i = 0; i < games; i++)
        {
            Game game = new Game(players);
            Player result = game.SimulateGame();
            if (result == Player.White) scores[0]++;
            else if (result == Player.Black) scores[1]++;
            // Result can also be NoPlayer = draw
        }
        return scores;
    }
}
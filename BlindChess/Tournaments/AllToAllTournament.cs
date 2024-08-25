using BlindChessEngine;

namespace BlindChess.Tournaments;

public class AllToAllTournament : AbstractTournament
{
    public AllToAllTournament(Dictionary<string, (IGameStrategy, IBlindStrategy)> participants, uint gamesPerPair)
        : base(participants, gamesPerPair)
    {}

    public override void Play()
    {
        foreach (var firstGame in Participants.Keys)
        foreach (var firstBlind in Participants.Keys)
        foreach (var secondGame in Participants.Keys)
        foreach (var secondBlind in Participants.Keys)
        {
            if (firstGame == secondGame || firstBlind == secondBlind)
                continue;
            PlaySeries(new [] {(firstGame, firstBlind), (secondGame, secondBlind)}.ToList());
        }
    }
}
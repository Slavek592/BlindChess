using BlindChessEngine;

namespace BlindChess.Tournaments;

public class PairTournament : AbstractTournament
{
    public PairTournament(Dictionary<string, (IGameStrategy, IBlindStrategy)> participants, uint gamesPerPair)
        : base(participants, gamesPerPair)
    {}

    public override void Play()
    {
        foreach (var participant in Participants.Keys)
        foreach (var competitor in Participants.Keys)
        {
            if (participant == competitor)
                continue;
            PlaySeries(new [] {(participant, participant), (competitor, competitor)}.ToList());
        }
    }
}
namespace BlindChessEngine;

public class Game
{
    private readonly Board _board;
    private readonly Dictionary<Player, (IGameStrategy gameStrategy, IBlindStrategy blindStrategy)> _players;

    public Game(Dictionary<Player, (IGameStrategy gameStrategy, IBlindStrategy blindStrategy)> players)
    {
        _board = new Board();
        _players = players;
        foreach (var player in _players.Keys)
        {
            _players[player].gameStrategy.StartGame(player);
            _players[player].blindStrategy.StartGame(player);
        }
    }

    public static Player GetOtherPlayer(Player player)
    {
        if (player == Player.White)
            return Player.Black;
        else
            return Player.White;
    }

    public Player SimulateGame()
    {
        uint j = 0;
        while (true)
        {
            j++;
            if (j > 200)
                return Player.NoPlayer;
            foreach (var player in _players)
            {
                FigureType chosenType = FigureType.NoType;
                bool valid = false;
                for (byte i = 0; i < 3; i++)
                {
                    chosenType = player.Value.blindStrategy.Guess();
                    if (_board.Figures[player.Key].All(figure => figure.FigureType != chosenType))
                        player.Value.blindStrategy.NoFigure();
                    else if (!_board.AbleToMove(player.Key, chosenType))
                        player.Value.blindStrategy.ImpossibleMove();
                    else
                    {
                        valid = true;
                        player.Value.blindStrategy.ValidMove();
                        break;
                    }
                }
                if (!valid)
                    continue;
                (Figure Figure, (byte, byte) Position) move = player.Value.gameStrategy.MakeMove(
                    _board.Figures[player.Key].Where(figure => figure.FigureType == chosenType).ToList(),
                    _board);
                if (move.Figure.FigureType != chosenType)
                    continue;
                Figure? eaten = _board.Move(move.Figure, move.Position);
                if (eaten is not null && eaten.FigureType == FigureType.King)
                    return player.Key;
                if (_board.CheckCheck(player.Key))
                    foreach (var strategies in _players.Values)
                        strategies.blindStrategy.Check(GetOtherPlayer(player.Key));
            }
        }
    }
}
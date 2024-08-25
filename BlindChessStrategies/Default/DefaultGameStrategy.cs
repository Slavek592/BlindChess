using BlindChessEngine;

namespace BlindChessStrategies.Default;

public class DefaultGameStrategy : IGameStrategy
{
    private Player _me;
    
    public void StartGame(Player player)
    {_me = player;}

    public (Figure Figure, (byte X, byte Y) Square) MakeMove(List<Figure> possibleFigures, Board board)
    {
        foreach (var figure in possibleFigures)
            foreach (var move in board.GetPossibleMoves(figure))
                if (board.Squares[move.X, move.Y] is not null &&
                    !board.CheckCheck(Game.GetOtherPlayer(_me), figure, move))
                    return (figure, move);
        foreach (var figure in possibleFigures)
            foreach (var move in board.GetPossibleMoves(figure))
                if (!board.CheckCheck(Game.GetOtherPlayer(_me), figure, move))
                    return (figure, move);
        
        throw new Exception("This should not happen.");
    }
}
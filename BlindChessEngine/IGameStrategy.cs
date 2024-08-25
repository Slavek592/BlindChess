namespace BlindChessEngine;

public interface IGameStrategy
{
    /// <summary>
    /// Says a new game has started and the strategy could prepare.
    /// </summary>
    /// /// <param name="player">The color of figures the strategy is playing.</param>
    public void StartGame(Player player);
    
    /// <summary>
    /// It has several possible figures to move, and it should find the best possible turn.
    /// </summary>
    /// <param name="possibleFigures">The list of figures of the type chosen by a blind strategy to choose from.</param>
    /// <param name="board">The game board.</param>
    /// <returns>The chosen figure and the square to move.</returns>
    public (Figure Figure, (byte X, byte Y) Square) MakeMove(List<Figure> possibleFigures, Board board);
}
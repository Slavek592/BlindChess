namespace BlindChessEngine;

public interface IBlindStrategy
{
    /// <summary>
    /// It is run every time a new game starts and says the strategy to restart its "knowledge".
    /// </summary>
    /// <param name="player">The color of figures the strategy is playing.</param>
    public void StartGame(Player player);
    
    /// <summary>
    /// This is the main method of the strategy, where it has to guess the best possible Figure type to move.
    /// </summary>
    /// <returns>FigureType</returns>
    public FigureType Guess();

    /// <summary>
    /// Well, it can guess wrong and this is when it is acknowledged.
    /// Three impossible moves mean the player loses its turn.
    /// </summary>
    public void ImpossibleMove();

    /// <summary>
    /// Special type of ImpossibleMove, when you have no more figures of this type.
    /// </summary>
    public void NoFigure();

    /// <summary>
    /// It returned some FigureType and the turn was OK.
    /// </summary>
    public void ValidMove();
    
    /// <summary>
    /// The king was attacked!
    /// </summary>
    /// <param name="player">Whose king?</param>
    public void Check(Player player);
}
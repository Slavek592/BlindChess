using BlindChessEngine;

namespace BlindChessStrategies.Default;

public class DefaultBlindStrategy : IBlindStrategy
{
    public void StartGame(Player player) {}

    public FigureType Guess()
    {
        return (FigureType)Random.Shared.Next(0, 5);
    }

    public void ImpossibleMove() {}

    public void NoFigure() {}

    public void ValidMove() {}
    
    public void Check(Player player) {}
}
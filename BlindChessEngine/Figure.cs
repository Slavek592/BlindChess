namespace BlindChessEngine;

public enum FigureType
{
    Pawn,
    Bishop,
    Knight,
    Rook,
    Queen,
    King,
    NoType
}

public class Figure(Player player, FigureType figureType, (byte X, byte Y) square)
{
    public readonly Player Player = player;
    public readonly FigureType FigureType = figureType;
    public (byte X, byte Y) Square = square;
}

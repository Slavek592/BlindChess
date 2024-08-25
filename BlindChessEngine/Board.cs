namespace BlindChessEngine;

public class Board
{
    public Figure?[,] Squares { get; }
    internal Dictionary<Player, List<Figure>> Figures { get; }
    
    internal Board()
    {
        var board = new Figure?[8, 8];
        
        for (byte column = 0; column < 8; column++)
            board[column, 1] = new Figure(Player.White, FigureType.Pawn, (column, 1));
        for (byte column = 0; column < 8; column++)
            board[column, 6] = new Figure(Player.Black, FigureType.Pawn, (column, 6));
        
        board[0, 0] = new Figure(Player.White, FigureType.Rook, (0, 0));
        board[7, 0] = new Figure(Player.White, FigureType.Rook, (7, 0));
        board[0, 7] = new Figure(Player.Black, FigureType.Rook, (0, 7));
        board[7, 7] = new Figure(Player.Black, FigureType.Rook, (7, 7));
        
        board[1, 0] = new Figure(Player.White, FigureType.Knight, (1, 0));
        board[6, 0] = new Figure(Player.White, FigureType.Knight, (6, 0));
        board[1, 7] = new Figure(Player.Black, FigureType.Knight, (1, 7));
        board[6, 7] = new Figure(Player.Black, FigureType.Knight, (6, 7));
        
        board[2, 0] = new Figure(Player.White, FigureType.Bishop, (2, 0));
        board[5, 0] = new Figure(Player.White, FigureType.Bishop, (5, 0));
        board[2, 7] = new Figure(Player.Black, FigureType.Bishop, (2, 7));
        board[5, 7] = new Figure(Player.Black, FigureType.Bishop, (5, 7));
        
        board[3, 0] = new Figure(Player.White, FigureType.Queen, (3, 0));
        board[4, 0] = new Figure(Player.White, FigureType.King, (4, 0));
        board[3, 7] = new Figure(Player.Black, FigureType.Queen, (3, 7));
        board[4, 7] = new Figure(Player.Black, FigureType.King, (4, 7));

        Squares = board;

        Figures = new Dictionary<Player, List<Figure>>();
        foreach (Player color in new[] {Player.White, Player.Black})
        {
            Figures[color] = new List<Figure>();
        }
        foreach (Figure? figure in Squares)
            if (figure is not null)
                Figures[figure.Player].Add(figure);
    }

    private Board(Figure?[,] squares, Dictionary<Player, List<Figure>> figures)
    {
        Squares = squares;
        Figures = figures;
    }

    public bool AbleToMove(Player player, FigureType figureType)
    {
        foreach (var figure in Figures[player].Where(f => f.FigureType == figureType))
        foreach (var move in GetPossibleMoves(figure))
            if (!CheckCheck(Game.GetOtherPlayer(player), figure, move))
                return true;
        return false;
    }

    public List<(byte X, byte Y)> GetPossibleMoves(Figure figure)
    {
        List<(byte X, byte Y)> SquaresInDirection(
            (byte X, byte Y) start, Direction direction, Player playerOwningFigure)
        {
            short newX = start.X;
            short newY = start.Y;
            switch (direction)
            {
                case Direction.Up:
                {
                    newY -= 1;
                    break;
                }
                case Direction.Down:
                {
                    newY += 1;
                    break;
                }
                case Direction.Left:
                {
                    newX -= 1;
                    break;
                }
                case Direction.Right:
                {
                    newX += 1;
                    break;
                }
                case Direction.UpLeft:
                {
                    newX -= 1;
                    newY -= 1;
                    break;
                }
                case Direction.UpRight:
                {
                    newX += 1;
                    newY -= 1;
                    break;
                }
                case Direction.DownLeft:
                {
                    newX -= 1;
                    newY += 1;
                    break;
                }
                case Direction.DownRight:
                {
                    newX += 1;
                    newY += 1;
                    break;
                }
            }
            if (newX < 0 || newX >= 8 || newY < 0 || newY >= 8 ||
                (Squares[newX, newY] != null && Squares[newX, newY]!.Player == playerOwningFigure))
                return new List<(byte, byte)>();
            else if (Squares[newX, newY] is not null)
                return new[] {(Convert.ToByte(newX), Convert.ToByte(newY))}.ToList();
            else
            {
                var nextSquares = SquaresInDirection((Convert.ToByte(newX), Convert.ToByte(newY)),
                    direction, playerOwningFigure);
                nextSquares.Add((Convert.ToByte(newX), Convert.ToByte(newY)));
                return nextSquares;
            }
        }
        
        List<(byte X, byte Y)> possibleMoves = new();
        switch (figure.FigureType)
        {
            case FigureType.Pawn:
            {
                byte newY = Convert.ToByte(figure.Player == Player.White ? figure.Square.Y + 1 : figure.Square.Y - 1);
                if (Squares[figure.Square.X, newY] is null)
                    possibleMoves.Add((figure.Square.X, newY));
                if (figure.Square.X > 0 && Squares[figure.Square.X - 1, newY] is not null
                                        && Squares[figure.Square.X - 1, newY]!.Player != figure.Player)
                    possibleMoves.Add((Convert.ToByte(figure.Square.X - 1), newY));
                if (figure.Square.X < 7 && Squares[figure.Square.X + 1, newY] is not null
                                        && Squares[figure.Square.X + 1, newY]!.Player != figure.Player)
                    possibleMoves.Add((Convert.ToByte(figure.Square.X + 1), newY));
                break;
            }
            case FigureType.Rook:
            {
                foreach (var direction in new [] {Direction.Up , Direction.Down, Direction.Left, Direction.Right})
                foreach (var square in SquaresInDirection(figure.Square, direction, figure.Player))
                    possibleMoves.Add(square);
                break;
            }
            case FigureType.Bishop:
            {
                foreach (var direction in new [] {Direction.UpLeft , Direction.DownLeft,
                             Direction.UpRight, Direction.DownRight})
                foreach (var square in SquaresInDirection(figure.Square, direction, figure.Player))
                    possibleMoves.Add(square);
                break;
            }
            case FigureType.Queen:
            {
                foreach (var direction in new [] {Direction.Up , Direction.Down, Direction.Left, Direction.Right,
                             Direction.UpLeft , Direction.DownLeft, Direction.UpRight, Direction.DownRight})
                foreach (var square in SquaresInDirection(figure.Square, direction, figure.Player))
                    possibleMoves.Add(square);
                break;
            }
            case FigureType.Knight:
            {
                foreach (var newX in new [] {-2 + figure.Square.X, 2 + figure.Square.X})
                foreach (var newY in new [] {-1 + figure.Square.Y, 1 + figure.Square.Y})
                    if (newX is >= 0 and < 8 && newY is >= 0 and < 8 &&
                        (Squares[newX, newY] is null || Squares[newX, newY]!.Player != figure.Player))
                        possibleMoves.Add((Convert.ToByte(newX), Convert.ToByte(newY)));
                foreach (var newX in new [] {-1 + figure.Square.X, 1 + figure.Square.X})
                foreach (var newY in new [] {-2 + figure.Square.Y, 2 + figure.Square.Y})
                    if (newX is >= 0 and < 8 && newY is >= 0 and < 8 &&
                        (Squares[newX, newY] is null || Squares[newX, newY]!.Player != figure.Player))
                        possibleMoves.Add((Convert.ToByte(newX), Convert.ToByte(newY)));
                break;
            }
            case FigureType.King:
            {
                foreach (var newX in new [] {-1 + figure.Square.X, figure.Square.X, 1 + figure.Square.X})
                foreach (var newY in new [] {-1 + figure.Square.Y, figure.Square.Y, 1 + figure.Square.Y})
                    if (newX is >= 0 and < 8 && newY is >= 0 and < 8 && (newX, newY) != figure.Square &&
                        (Squares[newX, newY] is null || Squares[newX, newY]!.Player != figure.Player))
                        possibleMoves.Add((Convert.ToByte(newX), Convert.ToByte(newY)));
                break;
            }
        }
        return possibleMoves;
    }

    internal Figure? Move(Figure figure, (byte X, byte Y) position)
    {
        if (!GetPossibleMoves(figure).Contains(position))
            throw new Exception("Invalid move!");
        if (CheckCheck(Game.GetOtherPlayer(figure.Player), figure, position))
            throw new Exception("The move leads to check!");
        
        Squares[figure.Square.X, figure.Square.Y] = null;
        Figure? eaten = Squares[position.X, position.Y];
        
        if (figure.FigureType == FigureType.Pawn && new[] { 0, 7 }.Contains(position.Y))
        {
            Figures[figure.Player].Remove(figure);
            figure = new Figure(figure.Player, FigureType.Queen, position);
            Figures[figure.Player].Add(figure);
        }
        else
            figure.Square = position;
        
        Squares[position.X, position.Y] = figure;
        
        if (eaten is not null)
            Figures[eaten.Player].Remove(eaten);
        return eaten;
    }

    private void TrustedMove(Figure figure, (byte X, byte Y) position)
    {
        Squares[figure.Square.X, figure.Square.Y] = null;
        Figure? eaten = Squares[position.X, position.Y];
        
        if (figure.FigureType == FigureType.Pawn && new[] { 0, 7 }.Contains(position.Y))
        {
            Figures[figure.Player].Remove(figure);
            figure = new Figure(figure.Player, FigureType.Queen, position);
            Figures[figure.Player].Add(figure);
        }
        else
            figure.Square = position;
        
        Squares[position.X, position.Y] = figure;
        
        if (eaten is not null)
            Figures[eaten.Player].Remove(eaten);
    }

    public bool CheckCheck(Player threat)
    {
        (byte, byte) enemyKingPosition =
            Figures[Game.GetOtherPlayer(threat)].First(f => f.FigureType == FigureType.King).Square;
        foreach (var figure in Figures[threat])
            if (GetPossibleMoves(figure).Any(p => p == enemyKingPosition))
                return true;
        return false;
    }
    
    // Well, it could be more time-effective.
    public bool CheckCheck(Player threat, Figure figure, (byte X, byte Y) position)
    {
        Figure?[,] possibleSquares = (Figure?[,])Squares.Clone();
        Dictionary<Player, List<Figure>> possibleFigures = new(Figures);
        foreach (var player in Figures.Keys)
            possibleFigures[player] = new List<Figure>(Figures[player]);
        possibleFigures[figure.Player].Remove(figure);
        Figure movedFigure = new Figure(figure.Player, figure.FigureType, figure.Square);
        possibleFigures[figure.Player].Add(movedFigure);
        Board possibleBoard = new Board(possibleSquares, possibleFigures);
        possibleBoard.TrustedMove(movedFigure, position);
        return possibleBoard.CheckCheck(threat);
    }
}

// [0, 0] is the most UpLeft
public enum Direction
{
    Up, Down, Left, Right,
    UpLeft, UpRight, DownLeft, DownRight
}

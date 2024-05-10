using System;
using Chess.Core;
using Microsoft.Xna.Framework;

namespace Chess.Util;
internal static class MoveValidator
{
    internal static bool CheckTheory(Move move)
    {   
        Vector2 direction = move.End - move.Start;
        return move.MovePieceType switch
        {
            PieceType.Pawn => 
                (move.IsCapture && Math.Abs(direction.X) == 1 && direction.Y == (move.OfWhite ? 1 : -1)) 
                || (!move.IsCapture && direction.Y == (move.OfWhite ? 1 : -1) && direction.X == 0)
                || (!move.IsCapture && direction.Y == 2 && move.OfWhite && move.Start.Y == 1 && direction.X == 0)
                || (!move.IsCapture && direction.Y == -2 && !move.OfWhite && move.Start.Y == 6 && direction.X == 0),
            PieceType.Rook => direction.X == 0 || direction.Y == 0,
            PieceType.Knight => 
            (Math.Abs(direction.X) == 1 && Math.Abs(direction.Y) == 2) || (Math.Abs(direction.X) == 2 && Math.Abs(direction.Y) == 1),
            PieceType.Bishop => Math.Abs(direction.X) == Math.Abs(direction.Y),
            PieceType.Queen => direction.X == 0 || direction.Y == 0 || Math.Abs(direction.X) == Math.Abs(direction.Y),
            PieceType.King => Math.Abs(direction.X) <= 1 && Math.Abs(direction.Y) <= 1,  
            _ => false,
        };
    }
    internal static bool CheckPath(Move move, Board board)
    {
        if (move.End == move.Start) return false;
        if (board.GetPieceAt(move.End) is not null && board.GetPieceAt(move.End).IsWhite == move.OfWhite) return false;
        Vector2 direction = move.End - move.Start;
        if (move.MovePieceType == PieceType.Knight) return true;
        Vector2 step = new(Math.Sign(direction.X), Math.Sign(direction.Y));
        for (var current = move.Start + step; current != move.End; current += step)
        {
            if (board.GetPieceAt(current) is not null) return false;
        }
        return true;
    }
}
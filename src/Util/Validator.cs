using System;
using Chess.Core;
using Microsoft.Xna.Framework;

namespace Chess.Util;
internal static class Validator
{
    internal static bool CheckTheory(Move move)
    {   
        Vector2 direction = move.End - move.Start;
        return move.MovePieceType switch
        {
            PieceType.Pawn => direction.Y == (move.OfWhite ? 1 : -1) && direction.X == 0,
            PieceType.Rook => direction.X == 0 || direction.Y == 0,
            PieceType.Knight => (Math.Abs(direction.X) == 1 && Math.Abs(direction.Y) == 2) || (Math.Abs(direction.X) == 2 && Math.Abs(direction.Y) == 1),
            PieceType.Bishop => Math.Abs(direction.X) == Math.Abs(direction.Y),
            PieceType.Queen => direction.X == 0 || direction.Y == 0 || Math.Abs(direction.X) == Math.Abs(direction.Y),
            PieceType.King => Math.Abs(direction.X) <= 1 && Math.Abs(direction.Y) <= 1,  
            _ => false,
        };
    }
}
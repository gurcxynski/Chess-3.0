using System;
using Chess.Core;
using Microsoft.Xna.Framework;

namespace Chess.Util;
internal static class MoveHelper
{
    internal static bool CheckPath(Vector2 start, Vector2 end, Board board)
    {
        if (end == start) return false;
        Vector2 direction = end - start;
        Vector2 step = new(Math.Sign(direction.X), Math.Sign(direction.Y));
        for (var current = start + step; current != end; current += step)
        {
            if (board.GetPieceAt(current) is not null) return false;
        }
        if (board.GetPieceAt(end) is not null && board.GetPieceAt(end).IsWhite == board.GetPieceAt(start).IsWhite) return false;
        return true;
    }
    internal static bool IsAttacked(Vector2 pos, Board board, bool white)
    {
        foreach (var piece in board.Pieces)
        {
            if (piece.IsWhite == white) continue;
            if (piece.CreateMove(pos, board) is not null) return true;
        }
        return false;
    }
}
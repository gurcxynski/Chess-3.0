using Chess.Core.Engine;
using Chess.Core.Engine.Pieces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Chess.Core.Util;
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
        return true;
    }

    internal static bool CheckDestination(Vector2 start, Vector2 dest, Board board)
    {
        return !(board.GetPieceAt(dest) is not null && board.GetPieceAt(dest).IsWhite == board.GetPieceAt(start).IsWhite);
    }
     
    internal static bool IsAttackedBy(Vector2 pos, Board board, bool white, bool verifyCheck = true)
    {
        var piecesCopy = new List<Piece>(board.Pieces);
        foreach (var piece in piecesCopy)
        {
            if (piece.IsWhite != white) continue;
            if (piece.TryCreatingMove(pos, board, verifyCheck) is not null) return true;
        }
        return false;
    }
    internal static bool WillBeChecked(Move move, Board board)
    {
        board.ExecuteMove(move);
        bool ret = IsAttackedBy(board.GetKing(!board.WhiteToMove).Position, board, board.WhiteToMove, false);
        board.UndoMove();
        return ret;
    }
    internal static bool IsEnPassant(Vector2 origin, Vector2 target, Board board)
    {
        if (!((target - origin).Y == 1 && Math.Abs((target - origin).X) == 1)) return false;
        Move lastMove = board.LastMove;
        return lastMove is not null && lastMove.MovedPiece is Pawn && lastMove.IsFirstMoveOfPiece &&
            (lastMove.End.X == target.X) && lastMove.End.Y == origin.Y;
    }
    internal static Piece GetPieceCapturedByEnPassant(Vector2 target, Board board)
    {
        if (board.WhiteToMove) return board.GetPieceAt(target - Vector2.UnitY);
        return board.GetPieceAt(target + Vector2.UnitY);
    }
}
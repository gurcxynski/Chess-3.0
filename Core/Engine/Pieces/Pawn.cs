using Chess.Core.Util;
using Microsoft.Xna.Framework;
using System;

namespace Chess.Core.Engine.Pieces;
internal class Pawn(Vector2 position, bool isWhite = true) : Piece(position, isWhite)
{
    protected override bool CheckBasicMovement(Vector2 direction, Board board)
    {
        bool isCapture = board.GetPieceAt(Position + direction) is not null;
        if (!IsWhite) direction.Y *= -1;
        if (direction == Vector2.UnitY && !isCapture) return true;
        if (direction == Vector2.UnitY * 2 && !HasMoved && !isCapture) return true;
        if (isCapture && direction.Y == 1 && Math.Abs(direction.X) == 1) return true;
        if (IsEnPassant(direction, board)) return true;
        return false;
    }
    private bool IsEnPassant(Vector2 direction, Board board)
    {
        if (!(direction.Y == 1 && Math.Abs(direction.X) == 1)) return false;
        Move lastMove = board.LastMove;
        return lastMove is not null && lastMove.MovedPiece is Pawn && lastMove.IsFirstMoveOfPiece && 
            (lastMove.End.X == (Position + direction).X) && lastMove.End.Y == Position.Y;
    }
}
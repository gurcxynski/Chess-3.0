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
        if (MoveHelper.IsEnPassant(Position, Position + direction, board)) return true;
        return false;
    }
}
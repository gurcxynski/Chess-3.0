using Chess.Core.Util;
using Microsoft.Xna.Framework;
using System.ComponentModel;

namespace Chess.Core.Engine.Pieces;
internal class Knight(Vector2 position, bool isWhite = true) : Piece(position, isWhite)
{
    internal override Move CreateMove(Vector2 target, Board board, bool verifyCheck = true)
    {
        if (IsCaptured) return null;
        var direction = target - Position;
        if (!((System.Math.Abs(direction.X) == 1 && System.Math.Abs(direction.Y) == 2)
        || (System.Math.Abs(direction.X) == 2 && System.Math.Abs(direction.Y) == 1))) return null;
        if (board.GetPieceAt(target)?.IsWhite == IsWhite) return null;
        var move = new Move(Position, target, this, board.GetPieceAt(target), firstMove: !HasMoved);
        if (verifyCheck && MoveHelper.WillBeChecked(move, board)) return null;
        return move;
    }
}

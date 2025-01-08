using Chess.Core.Util;
using Microsoft.Xna.Framework;

namespace Chess.Core.Engine.Pieces;
internal class Bishop : Piece
{
    internal Bishop(Vector2 position, bool isWhite = true) : base(position, isWhite) { }
    internal override Move CreateMove(Vector2 target, Board board, bool verifyCheck = true)
    {
        if (IsCaptured) return null;
        var direction = target - Position;
        if (!(System.Math.Abs(direction.X) == System.Math.Abs(direction.Y))) return null;
        if (!MoveHelper.CheckPath(Position, target, board)) return null;
        var move = new Move(Position, target, board, firstMove: !HasMoved);
        if (verifyCheck && MoveHelper.WillBeChecked(move, board)) return null;
        return move;
    }
}
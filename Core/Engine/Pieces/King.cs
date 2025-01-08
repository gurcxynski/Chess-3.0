using Chess.Core.Util;
using Microsoft.Xna.Framework;

namespace Chess.Core.Engine.Pieces;
internal class King : Piece
{
    internal King(Vector2 position, bool isWhite = true) : base(position, isWhite) { }
    internal override Move CreateMove(Vector2 target, Board board, bool verifyCheck = true)
    {
        if (IsCaptured) return null;
        var direction = target - Position;
        bool castles = false;
        if (System.Math.Abs(direction.X) == 2 && direction.Y == 0 && Position.Y == (IsWhite ? 0 : 7) && !HasMoved)
        {
            castles = true;
            for (var square = Position; square != target; square.X += System.Math.Sign(direction.X))
            {
                if (MoveHelper.IsAttackedBy(square, board, !IsWhite)) return null;
            }
            if (MoveHelper.IsAttackedBy(Position, board, !IsWhite)) return null;
        }
        if (!castles && !(System.Math.Abs(direction.X) <= 1 && System.Math.Abs(direction.Y) <= 1)) return null;
        if (MoveHelper.IsAttackedBy(target, board, !IsWhite)) return null;
        if (!MoveHelper.CheckPath(Position, target, board)) return null;
        var move = new Move(Position, target, board, castles: castles, firstMove: !HasMoved);
        if (verifyCheck && MoveHelper.WillBeChecked(move, board)) return null;
        return move;
    }
}

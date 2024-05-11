using Chess.Core;
using Chess.Util;
using Microsoft.Xna.Framework;

namespace Chess.Pieces
{
    internal class King : Piece
    {
        internal King(Vector2 position, bool isWhite = true) : base(position, isWhite) { }
        internal override Move CreateMove(Vector2 target, Board board) {
            var direction = target - Position;
            bool castles = false;
            if (System.Math.Abs(direction.X) == 2 && direction.Y == 0 && Position.Y == (IsWhite ? 0 : 7) && !HasMoved) {
                castles = true;
                for (var square = Position; square != target; square.X += System.Math.Sign(direction.X)) {
                    if (MoveHelper.IsAttacked(square, board, IsWhite)) return null;
                }
                if (MoveHelper.IsAttacked(Position, board, IsWhite)) return null;
            }
            if (!castles && !(System.Math.Abs(direction.X) <= 1 && System.Math.Abs(direction.Y) <= 1)) return null; 
            if (!MoveHelper.CheckPath(Position, target, board)) return null;
            return new Move(Position, target, board, castles);          
        }
    }
}
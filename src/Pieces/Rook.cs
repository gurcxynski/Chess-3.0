using Chess.Core;
using Chess.Util;
using Microsoft.Xna.Framework;

namespace Chess.Pieces
{
    internal class Rook : Piece
    {
        internal Rook(Vector2 position, bool isWhite = true) : base(position, isWhite) { }
        internal override Move CreateMove(Vector2 target, Board board) {
            var direction = target - Position;
            if (!(direction.X == 0 || direction.Y == 0)) return null;
            if (!MoveHelper.CheckPath(Position, target, board)) return null;
            return new Move(Position, target, board);          
        }
    }
}
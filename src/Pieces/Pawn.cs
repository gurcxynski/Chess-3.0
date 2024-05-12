using Chess.Core;
using Chess.Util;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Chess.Pieces
{
    internal class Pawn : Piece
    {
        internal Pawn(Vector2 position, bool isWhite = true) : base(position, isWhite) { }
        internal override Move CreateMove(Vector2 target, Board board) {
            var direction = target - Position;
            var IsCapture = board.GetPieceAt(target) != null;
            if (!IsWhite) direction *= -1;
            if (!IsCapture && !(direction.X == 0 && (direction.Y == 1 || (!HasMoved && direction.Y == 2)))) return null;
            if (IsCapture && !(System.Math.Abs(direction.X) == 1 && direction.Y == 1)) return null;
            if (!MoveHelper.CheckPath(Position, target, board)) return null;
            var move = new Move(Position, target, board, firstMove: !HasMoved);
            if (MoveHelper.WillBeChecked(move, board)) return null;
            return move;      
        }
    }
}
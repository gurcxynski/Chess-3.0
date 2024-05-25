using Chess.Util;
using Microsoft.Xna.Framework;

namespace Chess.Engine.Pieces;
internal class Knight : Piece
{
    internal Knight(Vector2 position, bool isWhite = true) : base(position, isWhite) { }
    internal override Move CreateMove(Vector2 target, Board board) {
        var direction = target - Position;
        if (!((System.Math.Abs(direction.X) == 1 && System.Math.Abs(direction.Y) == 2) 
        || (System.Math.Abs(direction.X) == 2 && System.Math.Abs(direction.Y) == 1))) return null;
        if (board.GetPieceAt(target)?.IsWhite == IsWhite) return null;
        var move = new Move(Position, target, board, firstMove: !HasMoved);
        if (MoveHelper.WillBeChecked(move, board)) return null;
        return move;        
    }
}

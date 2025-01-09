using Chess.Core.Util;
using Microsoft.Xna.Framework;

namespace Chess.Core.Engine.Pieces;
internal class Rook(Vector2 position, bool isWhite = true) : Piece(position, isWhite)
{
    internal override Move CreateMove(Vector2 target, Board board, bool verifyCheck = true)
    {
        if (IsCaptured) return null;
        var direction = target - Position;
        if (!(direction.X == 0 || direction.Y == 0)) return null;
        if (!MoveHelper.CheckPath(Position, target, board)) return null;
        var move = new Move(Position, target, this, board.GetPieceAt(target), firstMove: !HasMoved);
        if (verifyCheck && MoveHelper.WillBeChecked(move, board)) return null;
        return move;
    }
}
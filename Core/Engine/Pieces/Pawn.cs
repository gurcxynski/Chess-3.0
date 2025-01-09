using Chess.Core.Util;
using Microsoft.Xna.Framework;

namespace Chess.Core.Engine.Pieces;
internal class Pawn(Vector2 position, bool isWhite = true) : Piece(position, isWhite)
{
    internal override Move CreateMove(Vector2 target, Board board, bool verifyCheck = true)
    {
        if (IsCaptured) return null;
        var direction = target - Position;
        var IsCapture = board.GetPieceAt(target) != null;
        if (!IsWhite) direction.Y *= -1;
        var IsEnPassant = false;
        var lastMove = board.LastMove;
        if (lastMove != null)
        {
            var lastMovePiece = board.GetPieceAt(lastMove.End);
            if (lastMovePiece is Pawn && (lastMove.End - lastMove.Start).Length() == 2)
            {
                IsEnPassant = target == new Vector2(lastMove.End.X, lastMove.End.Y + (IsWhite ? 1 : -1));
                if (IsEnPassant)
                {
                    IsCapture = true;
                }
            }
        }
        if (!IsCapture && !(direction.X == 0 && (direction.Y == 1 || (!HasMoved && direction.Y == 2)))) return null;
        if (IsCapture && !(System.Math.Abs(direction.X) == 1 && direction.Y == 1)) return null;
        if (!MoveHelper.CheckPath(Position, target, board)) return null;
        var move = new Move(Position, target, board, firstMove: !HasMoved, enPassant: IsEnPassant);
        if (verifyCheck && MoveHelper.WillBeChecked(move, board)) return null;
        return move;
    }
}

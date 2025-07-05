using Backend.Util;
using System.Numerics;

namespace Backend.Engine.Pieces;

internal class King(Vector2 position, bool isWhite = true) : Piece(position, isWhite)
{
    protected override bool CheckBasicMovement(Vector2 direction, Board board)
    {
        if (Math.Abs(direction.X) <= 1 && Math.Abs(direction.Y) <= 1) return true;
        if (Math.Abs(direction.X) == 2 && direction.Y == 0 && Position.Y == (IsWhite ? 0 : 7) && !HasMoved)
        {
            if (MoveHelper.IsAttackedBy(Position, board, !IsWhite)) return false;
            for (var square = Position; square != Position + direction; square.X += Math.Sign(direction.X))
            {
                if (MoveHelper.IsAttackedBy(square, board, !IsWhite)) return false;
            }
            var piece = board.GetPieceAt(new Vector2(direction.X > 0 ? 7 : 0, Position.Y));
            return piece is not null && piece is Rook && !piece.HasMoved;
        }
        return false;
    }
}

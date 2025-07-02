using System.Numerics;

namespace Chess.Backend.Engine.Pieces;
internal class Knight(Vector2 position, bool isWhite = true) : Piece(position, isWhite)
{
    protected override bool CanJumpOver { get; } = true;
    protected override bool CheckBasicMovement(Vector2 direction, Board board) =>
        Math.Abs(direction.X) == 1 && Math.Abs(direction.Y) == 2
      || Math.Abs(direction.X) == 2 && Math.Abs(direction.Y) == 1;
}

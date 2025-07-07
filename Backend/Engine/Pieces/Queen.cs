using System.Numerics;

namespace Backend.Engine.Pieces;

internal class Queen(Vector2 position, bool isWhite = true) : Piece(position, isWhite)
{
	protected override bool CheckBasicMovement(Vector2 direction, Board board) => Math.Abs(direction.X) == Math.Abs(direction.Y) || direction.X == 0 || direction.Y == 0;
}
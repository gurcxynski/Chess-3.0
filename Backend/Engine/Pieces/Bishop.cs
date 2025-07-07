using System.Numerics;

namespace Backend.Engine.Pieces;

internal class Bishop(Vector2 position, bool isWhite = true) : Piece(position, isWhite)
{
	protected override bool CheckBasicMovement(Vector2 direction, Board board) => Math.Abs(direction.X) == Math.Abs(direction.Y);
}
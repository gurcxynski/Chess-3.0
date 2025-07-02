using System.Numerics;

namespace Chess.Backend.Engine.Pieces;
internal class Rook(Vector2 position, bool isWhite = true) : Piece(position, isWhite)
{
    protected override bool CheckBasicMovement(Vector2 direction, Board board) => direction.X == 0 || direction.Y == 0;
}
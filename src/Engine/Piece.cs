
using System;
using Microsoft.Xna.Framework;

namespace Chess.Engine;
internal abstract class Piece
{
    internal Vector2 Position { get; private set; }
    internal bool IsWhite { get; }
    internal bool HasMoved { get; set; } = false;
    internal bool IsCaptured { get; set; } = false;
    internal Piece(Vector2 position, bool isWhite = true)
    {
        Position = position;
        IsWhite = isWhite;
    }
    internal void Move(Vector2 pos)
    {
        Position = pos;
        HasMoved = true;
    }
    internal abstract Move CreateMove(Vector2 target, Board board);
}

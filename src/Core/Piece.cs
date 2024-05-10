using System;
using Microsoft.Xna.Framework;

namespace Chess.Core
{
    internal class Piece
    {
        internal PieceType Type { get; }
        internal Vector2 Position { get; private set; }
        internal bool IsWhite { get; }
        internal bool HasMoved { get; private set; } = false;
        internal Piece(PieceType type, Vector2 position, bool isWhite = true)
        {
            Type = type;
            Position = position;
            IsWhite = isWhite;
        }

        internal void Move(Vector2 pos)
        {
            Position = pos;
            HasMoved = true;
        }

        internal void Capture()
        {
            Position = new Vector2(-1, -1);
        }
    }

    internal enum PieceType
    {
        Pawn,
        Rook,
        Knight,
        Bishop,
        Queen,
        King
    }
}
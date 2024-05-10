using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chess.Core
{
    internal class Piece
    {
        private static int Size => Game1.Size;
        internal PieceType Type { get; }
        internal Vector2 GridPosition { get; private set; }
        internal Vector2 DrawPosition => Size * new Vector2(GridPosition.X, 7 - GridPosition.Y);
        internal Vector2 DrawPositionCentered => DrawPosition + Size / 2 * Vector2.One;
        internal bool IsWhite { get; }
        internal bool HasMoved { get; private set; } = false;
        internal Piece(PieceType type, Vector2 position, bool isWhite = true)
        {
            Type = type;
            GridPosition = position;
            IsWhite = isWhite;
        }
        internal void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = Game1.self.textures.Get((IsWhite ? "w" : "b") + Type.ToString());
            Rectangle destinationRectangle = new((int)DrawPosition.X, (int)DrawPosition.Y, Size, Size);
            spriteBatch.Draw(texture, destinationRectangle, texture.Bounds, Color.White);
        }

        internal void Move(Vector2 pos)
        {
            GridPosition = pos;
            HasMoved = true;
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
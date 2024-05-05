using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chess.Core
{
    internal class Piece
    {
        internal PieceType Type { get; init; }
        internal Vector2 GridPosition { get; }
        internal Vector2 DrawPosition => GridPosition * 100;
        internal Piece(PieceType type, Vector2 position)
        {
            Type = type;
            GridPosition = position;
        }
        internal void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = Game1.self.textures.Get(Type.ToString());
            Rectangle destinationRectangle = new((int)DrawPosition.X, (int)DrawPosition.Y, 100, 100);
            spriteBatch.Draw(texture, destinationRectangle, texture.Bounds, Color.White);
        }

        internal void Update(GameTime gameTime)
        {

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
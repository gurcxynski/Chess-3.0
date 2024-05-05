using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chess.Core
{
    internal class Piece
    {
        internal PieceType Type { get; }
        internal Vector2 GridPosition { get; private set; }
        internal Vector2 DrawPosition => 100 * GridPosition; //* new Vector2(1, -1) + 7 * Vector2.UnitY);
        internal Vector2 DrawPositionCentered => DrawPosition + new Vector2(50, 50);
        internal bool IsWhite { get; }
        internal Piece(PieceType type, Vector2 position, bool isWhite = true)
        {
            Type = type;
            GridPosition = position;
            IsWhite = isWhite;
        }
        internal void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = Game1.self.textures.Get(Type.ToString());
            Rectangle destinationRectangle = new((int)DrawPosition.X, (int)DrawPosition.Y, 100, 100);
            spriteBatch.Draw(texture, destinationRectangle, texture.Bounds, IsWhite ? Color.White : Color.Red);
        }

        internal void Update(GameTime gameTime)
        {

        }

        internal void Move(Vector2 pos)
        {
            GridPosition = pos;
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
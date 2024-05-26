using Chess.Engine;
using Chess.Util;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;

namespace Chess.UI;
internal class PieceIcon : Icon
{
    internal Piece Piece { get; private init; }
    internal PieceIcon(Piece piece) : base() {
        Anchor = Anchor.TopLeft;
        Piece = piece;
        Texture = Textures.Get(piece);
        Size = new Vector2(1/8f, 1/8f);
    }

    internal void Update(bool drawWhiteDown)
    {
        Offset = ToOffset(Piece.Position, drawWhiteDown);
        Visible = !Piece.IsCaptured;
        Draggable = Piece.IsWhite == ChessGame.Instance.WhiteTurn;
    }
    private Vector2 ToOffset(Vector2 vec, bool drawWhiteDown) {
        if (drawWhiteDown) vec = new Vector2(vec.X, 7 - vec.Y);
        return vec * ((Parent.Size - 2 * Parent.Padding) / 8);
    }
}
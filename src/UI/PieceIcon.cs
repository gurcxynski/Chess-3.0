using Chess.Engine;
using Chess.Util;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;

namespace Chess.UI;
internal class PieceIcon : Icon
{
    internal Piece Piece { get; private init; }
    public PieceIcon(Piece piece) : base() {
        Anchor = Anchor.TopLeft;
        Piece = piece;
        Texture = Textures.Get(piece);
        Size = new Vector2(0.125f, 0.125f);
    }
}
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
        LimitDraggingToParentBoundaries = false;
    }

    internal void Update(bool drawBlackDown, bool draggable)
    {
        Offset = PositionConverter.ToOffset(Piece.Position, Parent, drawBlackDown);
        Visible = !Piece.IsCaptured;
        Draggable = Visible && draggable;
    }
}
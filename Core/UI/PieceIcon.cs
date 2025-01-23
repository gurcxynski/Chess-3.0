using Chess.Core.Engine;
using Chess.Core.Util;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;

namespace Chess.Core.UI;
internal class PieceIcon : Icon
{
    internal Piece Piece { get; private init; }
    internal bool IsDragged { get; private set; }
    internal PieceIcon(Piece piece) : base()
    {
        Anchor = Anchor.TopLeft;
        Piece = piece;
        Texture = Resources.Instance.LoadTexture("pieces\\" + piece.ToString());
        Size = new Vector2(1 / 8f, 1 / 8f);
        LimitDraggingToParentBoundaries = false;
        OnStartDrag = (entity) =>
        {
            IsDragged = true;
        };
        OnStopDrag = (entity) =>
        {
            IsDragged = false;
        };
    }

    internal void Update(bool draggable)
    {
        Offset = PositionConverter.ToOffset(Piece.Position);
        Visible = !Piece.IsCaptured;
        Draggable = Visible && draggable;
    }
}
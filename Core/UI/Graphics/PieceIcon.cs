using Chess.Core.Engine;
using Chess.Core.Util;
using GeonBit.UI;
using GeonBit.UI.Entities;
namespace Chess.Core.UI.Graphics;
internal class PieceIcon : Icon
{
    internal Piece Piece { get; private init; }
    internal PieceIcon(Piece piece) : base()
    {
        Piece = piece;
        Anchor = Anchor.TopLeft;
        Texture = Resources.Instance.LoadTexture("pieces\\" + piece.ToString());
        Size = ChessGame.Instance.BoardSize / 8;
        LimitDraggingToParentBoundaries = false;
    }

    internal void Update(bool draggable)
    {
        Offset = PositionConverter.ToOffset(Piece.Position, !ChessGame.Instance.IsWhitePlayer);
        Visible = !Piece.IsCaptured;
        Draggable = Visible && draggable;
    }
}
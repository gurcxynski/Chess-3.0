using Chess.Core.UI;
using Microsoft.Xna.Framework;

namespace Chess.Core.GameTypes;

internal class HotSeatGame : ChessGame
{
    internal HotSeatGame(Vector2 size) : base(size, GameType.Hotseat) { }

    protected override bool IsDraggable(PieceIcon icon) => board.WhiteToMove == icon.Piece.IsWhite;

    protected override void PieceMovedByMouse(PieceIcon icon)
    {
        if (board.WhiteToMove != icon.Piece.IsWhite) return;
        base.PieceMovedByMouse(icon);
    }
    protected override void PiecePickedUp(PieceIcon icon)
    {
        if (board.WhiteToMove != icon.Piece.IsWhite) return;
        base.PiecePickedUp(icon);
    }
}
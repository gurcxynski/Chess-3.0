using System;
using Chess.UI;

namespace Chess;

internal class HotSeatGame : ChessGame
{
    internal HotSeatGame() : base(GameType.Hotseat)
    {
    }

    protected override bool IsDraggable(PieceIcon icon)
    {
        return board.WhiteToMove == icon.Piece.IsWhite;
    }

    protected override void PieceMovedByMouse (PieceIcon icon) {
        if (board.WhiteToMove != icon.Piece.IsWhite) return;
        base.PieceMovedByMouse(icon);
    }
    protected override void PiecePickedUp(PieceIcon icon) {
        if (board.WhiteToMove != icon.Piece.IsWhite) return;
        base.PiecePickedUp(icon);
    }
}
using Chess.UI;
using GeonBit.UI.Entities;

namespace Chess;

internal class OnlineGame : ChessGame {
    bool playingWhite;
    internal OnlineGame(bool white) : base(GameType.Online) {
        playingWhite = white;
        drawBlackDown = !white;
        AfterUpdate += (Entity entity) => {
            if (board.WhiteToMove == playingWhite) {
                ProcessMove(board.GetValidMoves()[0]);
            }
        };
    }

    protected override bool IsDraggable(PieceIcon icon)
    {
        return icon.Piece.IsWhite == playingWhite;
    }

    protected override void PieceMovedByMouse (PieceIcon icon) {
        if (board.WhiteToMove != playingWhite) return;
        base.PieceMovedByMouse(icon);
    }
    protected override void PiecePickedUp(PieceIcon piece) {
        if (board.WhiteToMove == playingWhite) return;
        base.PiecePickedUp(piece);
    }
}
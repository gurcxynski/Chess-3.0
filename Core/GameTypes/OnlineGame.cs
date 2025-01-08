using Chess.Core.UI;
using Microsoft.Xna.Framework;

namespace Chess.Core.GameTypes;

internal class OnlineGame : ChessGame
{
    readonly bool playingWhite;
    internal OnlineGame(Vector2 size, bool white) : base(size, GameType.Online)
    {
        playingWhite = white;
        drawBlackDown = !white;
        AfterUpdate += (entity) =>
        {
            if (board.WhiteToMove == playingWhite)
            {
                ProcessMove(board.GetValidMoves()[0]);
            }
        };
    }

    protected override bool IsDraggable(PieceIcon icon) => icon.Piece.IsWhite == playingWhite;

    protected override void PieceMovedByMouse(PieceIcon icon)
    {
        if (board.WhiteToMove != playingWhite) return;
        base.PieceMovedByMouse(icon);
    }
    protected override void PiecePickedUp(PieceIcon piece)
    {
        if (board.WhiteToMove == playingWhite) return;
        base.PiecePickedUp(piece);
    }
}
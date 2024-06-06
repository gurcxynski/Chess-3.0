using Chess.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;

namespace Chess;

internal class BotGame : ChessGame {
    readonly MyBot bot;
    internal BotGame(Vector2 size, bool white) : base(size, GameType.Bot) {
        bot = new MyBot(!white);
        drawBlackDown = !white;
        AfterUpdate += (Entity entity) => {
            if (board.WhiteToMove == bot.playingWhite) {
                var move = bot.Think(board);
                ProcessMove(move);
            }
        };
    }

    protected override bool IsDraggable(PieceIcon icon)
    {
        return icon.Piece.IsWhite != bot.playingWhite;
    }

    protected override void PieceMovedByMouse (PieceIcon icon) {
        if (board.WhiteToMove == bot.playingWhite) return;
        base.PieceMovedByMouse(icon);
    }
    protected override void PiecePickedUp(PieceIcon piece) {
        if (board.WhiteToMove == bot.playingWhite) return;
        base.PiecePickedUp(piece);
    }
}
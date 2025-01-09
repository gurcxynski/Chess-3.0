using Chess.Core.GameTypes;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;

namespace Chess.Core.UI;
internal partial class PlayArea : UserInterface
{
    readonly ChessGame game;
    internal PlayArea(Vector2 size, ChessGame.GameType type, bool white = true) : base()
    {
        ShowCursor = false;
        switch (type)
        {
            case ChessGame.GameType.Online:
                game = new OnlineGame(size, white);
                break;
            case ChessGame.GameType.Bot:
                game = new BotGame(size, white);
                break;
            case ChessGame.GameType.Hotseat:
                game = new HotSeatGame(size);
                break;
        }
        game.Anchor = Anchor.Center;
        game.SizeFactor = 0.9f;
        game.PaddingFactor = 0.025f;
        game.Init();


        AddEntity(game);
    }
}

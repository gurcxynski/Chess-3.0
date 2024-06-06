using System;
using System.Drawing;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;

namespace Chess.UI;
internal class PlayArea : UserInterface {
    readonly ChessGame game;
    internal PlayArea(Vector2 size, ChessGame.GameType type, bool white = true) : base() {
        switch (type) {
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
        game.Anchor = Anchor.CenterLeft;
        game.SizeFactor = 0.7f;
        game.PaddingFactor = 0.025f;
        game.Init();

        Panel upper = new(new(game.SizeFactor, (1 - game.SizeFactor) / 2), anchor: Anchor.TopLeft);
        Panel lower = new(new(game.SizeFactor, (1 - game.SizeFactor) / 2), anchor: Anchor.BottomLeft);

        PanelTabs tabs = new()
        {
            Size = new Vector2(1 - game.SizeFactor, 0.99f)
        };
        TabData tab = tabs.AddTab("Chat");

        tab = tabs.AddTab("Time");

        tabs.Anchor = Anchor.TopRight;

        AddEntity(tabs);
        AddEntity(upper);
        AddEntity(game);
        AddEntity(lower);
    }
}

using Chess.Core.GameTypes;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;

namespace Chess.Core.UI;
internal partial class PlayArea : UserInterface
{
    readonly ChessGame game;
    internal PlayArea(Vector2 size) : base()
    {
        ShowCursor = false;
        game = new HotSeatGame(size)
        {
            Anchor = Anchor.Center
        };
        game.Init();


        AddEntity(game);
    }
}

using Chess.Core.UI.Menus;
using GeonBit.UI;
using GeonBit.UI.Entities;

namespace Chess.Core.UI;
internal class PlayArea : UserInterface
{
    internal PlayArea() : base()
    {
        ShowCursor = false;
        var game = new ChessGame();
        AddEntity(game);
        game.Initialize();
    }
}
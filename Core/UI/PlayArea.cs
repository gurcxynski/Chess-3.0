using Chess.Core.UI.Menus;
using GeonBit.UI;

namespace Chess.Core.UI;
internal class PlayArea : UserInterface
{
    internal PlayArea() : base()
    {
        ShowCursor = false;
        var game = new ChessGame();
        game.Init();
        AddEntity(game);
        AddEntity(new MyButton("quit", StateMachine.ToMenu<StartMenu>));
    }
}
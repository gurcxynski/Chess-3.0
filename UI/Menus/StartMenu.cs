using Chess.Core;
using Chess.UI;

namespace Chess.UI.Menus;
internal class StartMenu : Menu
{
    public StartMenu() : base([
        new MyButton("New Game", StateMachine.ToMenu<NewGameMenu>),
        new MyButton("Options", StateMachine.ToMenu<OptionsMenu>),
        new MyButton("Quit", StateMachine.QuitGame)
    ])
    { }
}
namespace Chess.Core.UI.Menus;
internal class StartMenu : Menu
{
    public StartMenu() : base([
        new MyButton("Start Game", StateMachine.StartGame),
        new MyButton("Options", StateMachine.ToMenu<OptionsMenu>),
        new MyButton("Quit", StateMachine.QuitGame)
    ])
    { }
}
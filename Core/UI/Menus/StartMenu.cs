namespace Chess.Core.UI.Menus;
internal partial class StartMenu : Menu
{
    public StartMenu() : base([
        new MyButton("Start Game", StateMachine.ToMenu<NewGameMenu>),
        new MyButton("Options", StateMachine.ToMenu<OptionsMenu>),
        new MyButton("Quit Game", StateMachine.QuitGame),
    ])
    { }
}
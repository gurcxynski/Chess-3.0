namespace Chess.Core.UI.Menus;
internal class StartMenu : Menu
{
    public StartMenu() : base([
        new MyButton("New Game", StateMachine.ToMenu<NewGameMenu>),
        new MyButton("Options", StateMachine.ToMenu<OptionsMenu>),
        new MyButton("Engine Settings", StateMachine.ToMenu<EngineSettingsMenu>),
        new MyButton("Quit", StateMachine.QuitGame)
    ])
    { }
}
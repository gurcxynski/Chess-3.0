namespace Chess.Core.UI.Menus;
internal partial class NewGameMenu : Menu
{
    public NewGameMenu() : base([
        new MyButton("Play locally", StateMachine.ToMenu<LocalGameMenu>),
        new MyButton("Play online", () => { }),
        new MyButton("Back", StateMachine.ToMenu<StartMenu>),
    ])
    { }
}
using GeonBit.UI.Entities;

namespace Chess.Core.UI.Menus;
internal partial class OptionsMenu : Menu
{
    public OptionsMenu() : base([
        new RadioButton("Theme 1"),
        new RadioButton("Theme 2"),
        new RadioButton("Theme 3"),
        new MyButton("Back", StateMachine.ToMenu<StartMenu>),
    ])
    { }
}
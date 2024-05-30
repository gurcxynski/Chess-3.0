using System.Collections.Generic;
using GeonBit.UI.Entities;

namespace Chess.UI;
internal class OptionsMenu : Menu {
    public OptionsMenu() : base(new List<Entity> {
        new RadioButton("Theme 1"),
        new RadioButton("Theme 2"),
        new RadioButton("Theme 3"),
        new MyButton("Back", StateMachine.ToMenu<StartMenu>),
    }) {}
}
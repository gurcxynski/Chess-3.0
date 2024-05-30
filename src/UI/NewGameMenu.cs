using System.Collections.Generic;
using GeonBit.UI.Entities;

namespace Chess.UI;
internal class NewGameMenu : Menu {
    public NewGameMenu() : base(new List<Entity> {
        new MyButton("Play locally", StateMachine.StartGame),
        new MyButton("Play online", () => { }),
        new MyButton("Back", StateMachine.ToMenu<StartMenu>),
    }) {}
}
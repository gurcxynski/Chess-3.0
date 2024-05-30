using System.Collections.Generic;
using GeonBit.UI.Entities;

namespace Chess.UI;
internal class StartMenu : Menu {
    public StartMenu() : base(new List<Entity> {
        new MyButton("Start Game", StateMachine.ToMenu<NewGameMenu>),
        new MyButton("Options", StateMachine.ToMenu<OptionsMenu>),
        new MyButton("Quit Game", StateMachine.QuitGame),
    }) {}
}
using System.Collections.Generic;
using GeonBit.UI.Entities;

namespace Chess.UI;
internal class LocalGameMenu : Menu {
    public LocalGameMenu() : base(new List<Entity> {
        new MyButton("Hot-seat", () => { StateMachine.StartGame(); }),
        new MyButton("Against a bot, white", () => { StateMachine.StartGame(true); }),
        new MyButton("Against a bot, black", () => { StateMachine.StartGame(false); }),
    }) {}
}
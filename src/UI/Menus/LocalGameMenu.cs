using System.Collections.Generic;
using GeonBit.UI.Entities;

namespace Chess.UI;
internal class LocalGameMenu : Menu {
    public LocalGameMenu() : base(new List<Entity> {
        new MyButton("Hot-seat", () => { StateMachine.StartGame(ChessGame.GameType.Hotseat); }),
        new MyButton("Against a bot, white", () => { StateMachine.StartGame(ChessGame.GameType.Bot, true); }),
        new MyButton("Against a bot, black", () => { StateMachine.StartGame(ChessGame.GameType.Bot, false); }),
    }) {}
}
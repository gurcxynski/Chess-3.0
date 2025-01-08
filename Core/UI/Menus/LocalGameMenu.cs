namespace Chess.Core.UI.Menus;
internal partial class LocalGameMenu : Menu
{
    public LocalGameMenu() : base([
        new MyButton("Hot-seat", () => { StateMachine.StartGame(ChessGame.GameType.Hotseat); }),
        new MyButton("Against a bot, white", () => { StateMachine.StartGame(ChessGame.GameType.Bot, true); }),
        new MyButton("Against a bot, black", () => { StateMachine.StartGame(ChessGame.GameType.Bot, false); }),
    ])
    { }
}
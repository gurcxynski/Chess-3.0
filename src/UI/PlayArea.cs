using GeonBit.UI;

namespace Chess.UI;
internal class PlayArea : UserInterface {
    readonly ChessGame game;
    internal PlayArea(ChessGame.GameType type, bool white = true) : base() {
        AddEntity(new MyButton("Back", StateMachine.ToMenu<NewGameMenu>));
        switch (type) {
            case ChessGame.GameType.Online:
                game = new OnlineGame(white);
                break;
            case ChessGame.GameType.Bot:
                game = new BotGame(white);
                break;
            case ChessGame.GameType.Hotseat:
                game = new HotSeatGame();
                break;
        }
        game.UpdateIcons();
        AddEntity(game);
    }
}

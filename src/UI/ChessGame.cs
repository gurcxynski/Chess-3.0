using GeonBit.UI;

namespace Chess.UI;
internal class ChessGame : UserInterface {
    readonly PlayableArea game;
    internal ChessGame(bool white) : base() {
        AddEntity(new MyButton("Back", StateMachine.ToMenu<NewGameMenu>));
        game = new PlayableArea(white);
        AddEntity(game);
    }
    internal ChessGame() : base() {
        AddEntity(new MyButton("Back", StateMachine.ToMenu<NewGameMenu>));
        game = new PlayableArea();
        AddEntity(game);
    }
}

using GeonBit.UI;

namespace Chess.UI;
internal class ChessGame : UserInterface {
    readonly PlayableArea game = new();
    internal ChessGame() : base() {
        AddEntity(new MyButton("Back", StateMachine.ToMenu<NewGameMenu>));
        AddEntity(game);
    }
}

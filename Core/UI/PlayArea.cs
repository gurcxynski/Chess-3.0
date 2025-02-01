using GeonBit.UI;

namespace Chess.Core.UI;
internal class PlayArea : UserInterface
{
    internal PlayArea(bool white, IMoveReceiver receiver) : base()
    {
        ShowCursor = false;
        var game = new ChessGame(receiver, white);
        AddEntity(game);
        game.Initialize();
    }
}
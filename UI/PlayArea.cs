using Chess.Core;
using Chess.UI.Menus;
using GeonBit.UI;

namespace Chess.UI;
internal class PlayArea : UserInterface
{
    internal PlayArea(GameCreator.GameData data, IMoveReceiver receiver) : base()
    {
        var game = new ChessGame(receiver, data);
        AddEntity(game);
        game.Initialize();
    }
}
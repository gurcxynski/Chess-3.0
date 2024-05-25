using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;

namespace Chess.UI;
internal class NewGameMenu : Panel {
    internal NewGameMenu() : base(new Vector2(0.7f, 0.5f)) {
        Button button = new("Start Game")
            {
                OnClick = (Entity entity) =>
                {
                    UserInterface.Active.RemoveEntity(this);
                    UserInterface.Active.AddEntity(new ChessGame());
                }
            };
        AddChild(button);
    }
}
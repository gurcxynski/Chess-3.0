
using ;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;

namespace Chess.UI;
internal class NewGameMenu : Panel {
    internal NewGameMenu() : base(new Vector2(500, 500)) {
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
using Microsoft.Xna.Framework;
using static Chess.UI.Button;

namespace Chess.UI;
internal class NewGameMenu : Menu {
    internal NewGameMenu() {
        buttons.Add(new Button(400, 200, Bounds.Location.ToVector2(), "button", new MyListener(Game1.self.machine.NewGame)));
    }
}
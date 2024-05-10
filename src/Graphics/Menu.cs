using System.Collections.Generic;
using Chess.Core;
using Microsoft.Xna.Framework.Graphics;

namespace Chess.Graphics;
internal abstract class Menu : GameScreen
{
    protected readonly List<Button> buttons = new();
    internal override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);
        buttons.ForEach(button => button.Draw(spriteBatch));
    }
}

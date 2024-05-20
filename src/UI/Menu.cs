using System.Collections.Generic;
using Chess.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chess.UI;
internal abstract class Menu : GameScreen
{
    internal Menu() {
        Background = Game1.self.textures.Get("brown");
        Bounds = new Rectangle(0, 0, 8 * Game1.Size, 8 * Game1.Size);
    } 
    protected readonly List<Button> buttons = new();
    internal override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);
        buttons.ForEach(button => button.Draw(spriteBatch));
    }
    internal override void Update(GameTime gameTime) {
        buttons.ForEach(button => button.Update(gameTime));
    }
}

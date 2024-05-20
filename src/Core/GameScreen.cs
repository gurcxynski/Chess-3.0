using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chess.Core;
internal abstract class GameScreen
{
    protected Texture2D Background { get; init; }
    protected Rectangle Bounds { get; init; }
    internal abstract void Update(GameTime gameTime);
    internal virtual void Draw(SpriteBatch spriteBatch) => spriteBatch.Draw(Background, Bounds, Color.White);
}

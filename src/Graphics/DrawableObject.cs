using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chess.Graphics;
internal abstract class DrawableObject
{
    protected Rectangle Bounds { get; set; }
    protected Texture2D Texture { get; set; }
    internal DrawableObject(Rectangle bounds, Texture2D texture, Vector2 parent)
    {
        Bounds = bounds;
        Bounds.Offset(parent);
        Texture = texture;
    }
    internal virtual void Draw(SpriteBatch spriteBatch) => spriteBatch.Draw(Texture, Bounds, Color.White);
    internal virtual void DrawAt(SpriteBatch spriteBatch, Vector2 pos) => spriteBatch.Draw(Texture, pos, Color.White);
}
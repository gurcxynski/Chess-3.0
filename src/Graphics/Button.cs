using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input.InputListeners;

namespace Chess.Graphics;

internal class Button : DrawableObject
{
    private readonly MouseListener mouseListener;
    internal interface IMyListener {
        internal void OnClick() {
            
        }
    }
    internal Button(Rectangle bounds, Texture2D texture, Vector2 parent, IMyListener myListener) : base(bounds, texture, parent)
    {
        mouseListener = new MouseListener();
        mouseListener.MouseClicked += (sender, args) => {
            if (bounds.Contains(args.Position)) {
                myListener.OnClick();
            }
        };
    }
    internal void Update(GameTime gameTime) => mouseListener.Update(gameTime);
}
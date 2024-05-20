using System;
using Chess.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input.InputListeners;

namespace Chess.UI;

internal class Button : DrawableObject
{
    private readonly MouseListener mouseListener;
    internal delegate void ClickAction();
    
    internal class MyListener
    {
        internal Action OnClick;
        internal MyListener(Action action) => OnClick += action;
    }
    internal Button(Rectangle bounds, Texture2D texture, Vector2 parent, MyListener listener) : base(bounds, texture, parent)
    {
        mouseListener = new MouseListener();
        mouseListener.MouseClicked += (sender, args) => {
            if (bounds.Contains(args.Position)) listener.OnClick();
        };
    }
    internal Button(int width, int height, Vector2 parent, string textureName, MyListener listener) 
    : base(new Rectangle((int)parent.X, (int)parent.Y, width, height), Game1.self.textures.Get(textureName), parent)
    {
        mouseListener = new MouseListener();
        mouseListener.MouseClicked += (sender, args) => {
            if (Bounds.Contains(args.Position)) listener.OnClick();
        };
    }
    internal void Update(GameTime gameTime) => mouseListener.Update(gameTime);
}
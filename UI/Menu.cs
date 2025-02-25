using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;
using System;
using System.Collections.Generic;

namespace Chess.UI;
internal class Menu : UserInterface
{
    private readonly Panel holder = new(Vector2.One * 0.8f, anchor: Anchor.Center) { Padding = new(80), MinSize = new(1100, 700) };
    internal UserInterface Previous;
    internal Menu(string name, IEnumerable<Entity> items) : base()
    {
        AddToPanel(new Header(name));
        foreach (var item in items)
        {
            AddToPanel(item);
        }
        AddEntity(holder);
    }
    protected static void OnKeyPressed(Keys key, Action action)
    {
        Core.Chess.keyboardListener.KeyReleased += (object sender, KeyboardEventArgs e) => { if (e.Key.Equals(key)) action(); };
    }
    protected void AddToPanel(Entity entity)
    {
        holder.AddChild(entity);
    }
}
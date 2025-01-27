using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;
using System;
using System.Collections.Generic;

namespace Chess.Core.UI;
internal class Menu : UserInterface
{
    private readonly Panel holder = new(Vector2.One * 0.5f, PanelSkin.Default, Anchor.Center);
    internal Menu(IEnumerable<Entity> items) : base()
    {
        ShowCursor = false;
        foreach (var item in items)
        {
            holder.AddChild(item);
        }
        AddEntity(holder);
        OnKeyPressed(Keys.Escape, StateMachine.Back);
    }
    protected static void OnKeyPressed(Keys key, Action action)
    {
        Chess.keyboardListener.KeyReleased += (object sender, KeyboardEventArgs e) =>
        {
            if (e.Key.Equals(key))
            {
                action();
            }
        };
    }

    protected void AddToPanel(Entity entity)
    {
        holder.AddChild(entity);
    }
}
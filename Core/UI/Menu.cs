using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Input.InputListeners;
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
        Chess.keyboardListener.KeyReleased += (object sender, KeyboardEventArgs e) =>
        {
            switch (e.Key)
            {
                case Microsoft.Xna.Framework.Input.Keys.Escape:
                    StateMachine.Back();
                    break;
            }
        };
    }

    protected void AddToPanel(Entity entity)
    {
        holder.AddChild(entity);
    }
}
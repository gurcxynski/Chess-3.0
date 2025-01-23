using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Chess.Core.UI;
internal class Menu : UserInterface
{
    private readonly Panel holder = new(Vector2.One * 0.7f, PanelSkin.Default, Anchor.Center);
    internal Menu(IEnumerable<Entity> items) : base()
    {
        ShowCursor = false;
        foreach (var item in items)
        {
            holder.AddChild(item);
        }
        AddEntity(holder);
    }
}
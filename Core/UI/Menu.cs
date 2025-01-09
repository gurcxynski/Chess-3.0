using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Chess.Core.UI;
internal partial class Menu : UserInterface
{
    readonly Panel holder = new(new Vector2(0.6f, 0.6f), PanelSkin.Default, Anchor.Center);
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
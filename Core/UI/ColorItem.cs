using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;

namespace Chess.Core.UI;

internal class ColorItem : RadioButton
{
    private readonly ColoredRectangle rectangle = new()
    {
        ClickThrough = true,
        Anchor = Anchor.TopLeft,
        Size = Vector2.Zero,
        OutlineWidth = 5
    };
    public ColorItem(bool white) : base() {
        Size = new(0.3f, 0);
        Padding = Vector2.Zero;
        Checked = white;
        rectangle.OutlineColor = white ? Color.Black : Color.White;
        rectangle.AddChild(new Image() { ClickThrough = true, Texture = Resources.Instance.LoadTexture($"pieces\\{(white ? 'w' : 'b')}King"), Anchor = Anchor.Center });
        OnValueChange += (entity) =>
        {
            foreach (var child in Parent.Children) if (child is ColorItem item) item.rectangle.OutlineColor = item.Checked ? Color.Black : Color.White;
        };
        AddChild(rectangle);
    }
}
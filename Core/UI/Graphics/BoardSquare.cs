using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;

namespace Chess.Core.UI.Graphics;
internal class BoardSquare : ColoredRectangle
{
    internal enum HighlightType
    {
        Move,
        Capture,
        Check,
        LastMove,
    }
    internal static Color light = new(120, 67, 21);
    internal static Color dark = new(240, 155, 89);
    internal Vector2 Coordinates { get; private init; }
    internal BoardSquare(int x, int y)
    {
        FillColor = (x + y) % 2 == 0 ? dark : light;
        Coordinates = new(x, y);
        Size = Vector2.One / 8;
        Anchor = Anchor.AutoInline;
        SpaceAfter = Vector2.Zero;
        Padding = Vector2.Zero;
        SetStyleProperty("FillColor", new(new Color(0.7f, 0.7f, 0.7f)), EntityState.MouseHover);
    }
    internal void Highlight(HighlightType type)
    {
        UnHighlight();
        AddChild(new ColoredRectangle
        {
            FillColor = type switch
            {
                HighlightType.Move => Color.Blue,
                HighlightType.Capture => Color.Crimson,
                HighlightType.Check => Color.Red,
                HighlightType.LastMove => Color.RosyBrown,
                _ => FillColor
            },
            Opacity = 150,
            OutlineWidth = 0,
            Size = Vector2.Zero
        });
    }
    internal void UnHighlight()
    {
        ClearChildren();
    }
}
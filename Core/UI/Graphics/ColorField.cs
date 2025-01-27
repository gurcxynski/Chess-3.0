using Chess.Core.Engine;
using Chess.Core.Util;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;

namespace Chess.Core.UI.Graphics;

internal class ColorField : Image
{
    internal enum HighlightType
    {
        Move,
        Capture,
        Check
    }
    internal HighlightType Type { get; private init; }
    internal ColorField(Vector2 square, HighlightType type)
    {
        Texture = Resources.Instance.LoadTexture("circle");
        Type = type;
        Size = Vector2.One / 8;
        Anchor = Anchor.TopLeft;
        Offset = PositionConverter.ToOffset(square);
        FillColor = type switch
        {
            HighlightType.Move => Color.Blue,
            HighlightType.Capture => Color.Crimson,
            HighlightType.Check => Color.Red,
            _ => Color.Transparent
        };
        Opacity = type switch
        {
            HighlightType.Move => 200,
            HighlightType.Capture => 200,
            HighlightType.Check => 200,
            _ => 0
        };
        PriorityBonus = -50;
    }
}
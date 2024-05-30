using Chess.Util;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;

namespace Chess.UI
{
    internal class ColorField : ColoredRectangle
    {
        internal enum HighlightType
        {
            Move,
            Capture,
            Check,
            Checkmate
        }
        internal HighlightType Type { get; private init; }
        internal ColorField(Color color, Vector2 square, Entity parent, bool drawWhiteDown, HighlightType type) : base(color)
        {
            Type = type;
            Size = new Vector2(1 / 8f, 1 / 8f);
            Anchor = Anchor.TopLeft;
            Offset = PositionConverter.ToOffset(square, parent, drawWhiteDown);
            Opacity = 10;
            PriorityBonus = -50;
        }
    }
}
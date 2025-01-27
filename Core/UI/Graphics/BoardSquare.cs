using Chess.Core.Util;
using GeonBit.UI.DataTypes;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Chess.Core.UI.Graphics;
internal class BoardSquare : ColoredRectangle
{
    internal BoardSquare(bool light)
    {
        FillColor = light ? new(240, 155, 89) : new(120, 67, 21);
        Size = Vector2.One / 8;
        Anchor = Anchor.AutoInline;
        SpaceAfter = Vector2.Zero;
        //Offset = PositionConverter.ToOffset(square);
        //FillColor = type switch
        //{
        //    HighlightType.Move => Color.Blue,
        //    HighlightType.Capture => Color.Crimson,
        //    HighlightType.Check => Color.Red,
        //    _ => Color.Transparent
        //};
        PriorityBonus = -50;
        var color = new Color(0.7f, 0.7f, 0.7f);
        SetStyleProperty("FillColor", new(color), EntityState.MouseHover);
        SetStyleProperty("Opacity", new(100), EntityState.MouseDown);


    }
}
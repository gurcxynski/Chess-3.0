using GeonBit.UI.Entities;

namespace Chess.Core.UI;

internal class ColorSelector : Panel
{
    readonly ColorItem white;
    readonly ColorItem black;
    internal bool WhiteSelected => white.Checked;
    public ColorSelector() : base()
    {
        Size = new Microsoft.Xna.Framework.Vector2(0.6f, 0.3f);
        Anchor = Anchor.AutoCenter;
        white = new ColorItem(true)
        {
            Anchor = Anchor.CenterLeft,
        };
        black = new ColorItem(false)
        {
            Anchor = Anchor.CenterRight,
        };
        AddChild(white);
        AddChild(black);
    }
}

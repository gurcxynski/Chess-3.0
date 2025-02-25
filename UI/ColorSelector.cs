using GeonBit.UI.Entities;

namespace Chess.UI;

internal class ColorSelector : Entity
{
    readonly ColorItem white;
    readonly ColorItem black;
    internal bool SelectedWhite => white.Checked;
    internal ColorSelector() : base()
    {
        Size = new(0);
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

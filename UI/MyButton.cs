using GeonBit.UI.Entities;
using System;

namespace Chess.UI;
internal class MyButton : Button
{
    internal MyButton(string text, Action onClick) : base(text)
    {
        Anchor = Anchor.AutoCenter;
        Size = new(0.5f, 0.2f);
        OnClick = (entity) => { onClick(); };
    }
}
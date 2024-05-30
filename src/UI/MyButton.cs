using System;
using GeonBit.UI.Entities;

namespace Chess.UI;
internal class MyButton : Button {
    internal MyButton(string text, Action onClick) : base(text) {
        OnClick = (Entity entity) => {
            onClick();
        };
    }
}
using GeonBit.UI.Entities;
using System;

namespace Chess.Core.UI;
internal class MyButton : Button
{
    internal MyButton(string text, Action onClick) : base(text) => OnClick = (entity) => { onClick(); };
}
using GeonBit.UI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core.UI
{
    internal class ColorSelector : Panel
    {
        readonly RadioButton white = new("white");
        readonly RadioButton black = new("black");
        readonly RadioButton random = new("random");
        internal bool WhiteSelected => white.Checked || (random.Checked && new Random().Next(2) == 0);
        public ColorSelector() : base()
        {
            Size = new Microsoft.Xna.Framework.Vector2(0.9f, 0.5f);
            white.Checked = true;
            AddChild(white);
            AddChild(black);
            AddChild(random);
        }
    }
}

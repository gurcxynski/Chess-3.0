using GeonBit.UI.Entities;
using System;
using static Chess.Core.EngineIntegration;

namespace Chess.UI.EngineOptions
{
    internal abstract class EngineOption : Entity
    {
        internal EventHandler<(string, string)> OnOptionChanged;
        internal EventHandler<string> OnButtonPressed;
        public EngineOption(Option option)
        {
            Size = new(0.9f, 0.2f);
            Anchor = Anchor.AutoCenter;
            OutlineWidth = 0;
        }
    }
}

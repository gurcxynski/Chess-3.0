using GeonBit.UI.Entities;
using System;
using static Chess.Core.EngineIntegration;

namespace Chess.UI.EngineOptions
{
    internal abstract class EngineOption : Panel
    {
        internal EventHandler<(string, string)> OnOptionChanged;
        internal EventHandler<string> OnButtonPressed;
        public EngineOption(Option option)
        {
            Size = new(0f, 0.15f);
            AdjustHeightAutomatically = true;
            Anchor = Anchor.AutoCenter;
        }
    }
}

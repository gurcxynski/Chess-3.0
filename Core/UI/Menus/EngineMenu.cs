using GeonBit.UI.Entities;
using System;
using System.Collections.Generic;

namespace Chess.Core.UI.Menus
{
    internal class EngineMenu : Menu
    {

        public EngineMenu() : base([])
        {
            ColorSelector colorSelector = new();
            AddToPanel(colorSelector);
            Slider elo = new(10, 30);
            AddToPanel(elo);
            EngineIntegration engineIntegration = new(colorSelector.WhiteSelected, elo.Value * 1000);
            AddToPanel(new MyButton("Start", () => {
                engineIntegration.Start();
                StateMachine.StartGame(colorSelector.WhiteSelected, engineIntegration);
            }));
            AddToPanel(new MyButton("Back", StateMachine.Back));
        }
    }
}
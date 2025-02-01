using GeonBit.UI.Entities;
using System.Collections.Generic;

namespace Chess.Core.UI.Menus
{
    internal class EngineSettingsMenu : Menu
    {
        public EngineSettingsMenu() : base([])
        {
            AddToPanel(new MyButton("Back", StateMachine.Back));
        }
    }
}
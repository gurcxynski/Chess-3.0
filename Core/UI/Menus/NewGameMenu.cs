using GeonBit.UI.Entities;
using System.Collections.Generic;

namespace Chess.Core.UI.Menus
{
    internal class NewGameMenu : Menu
    {
        public NewGameMenu() : base([])
        {
            AddToPanel(new MyButton("Play against a chess engine", StateMachine.ToMenu<EngineMenu>));
            AddToPanel(new MyButton("Play against a network opponent", StateMachine.ToMenu<OnlineMenu>));
            AddToPanel(new MyButton("Back", StateMachine.Back));
        }
    }
}
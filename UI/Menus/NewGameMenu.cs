using Chess.Core;
using Chess.UI;
using GeonBit.UI.Entities;
using System.Collections.Generic;

namespace Chess.UI.Menus
{
    internal class NewGameMenu : Menu
    {
        public NewGameMenu() : base("New Game", [])
        {
            AddToPanel(new MyButton("Play against a chess engine", StateMachine.ToMenu<EngineMenu>));
            AddToPanel(new MyButton("Play against a network opponent", StateMachine.ToMenu<OnlineMenu>));
            AddToPanel(new MyButton("Back", StateMachine.ToMenu<StartMenu>));
        }
    }
}
using GeonBit.UI.Entities;

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
            AddToPanel(new MyButton("Start", () => {
                EngineIntegration engineIntegration = new(colorSelector.WhiteSelected, elo.Value * 100);
                engineIntegration.Start();
                StateMachine.StartGame(colorSelector.WhiteSelected, engineIntegration);
            }));
            AddToPanel(new MyButton("Back", StateMachine.Back));
        }
    }
}
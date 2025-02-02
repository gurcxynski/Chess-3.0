using GeonBit.UI.Entities;

namespace Chess.Core.UI.Menus;

internal class EngineMenu : Menu
{
    public EngineMenu() : base([])
    {
        ColorSelector colorSelector = new();
        Paragraph eloText = new();
        Slider elo = new(0, 20)
        {
            Value = 10,
            Size = new(0.5f, 0.1f),
            Anchor = Anchor.AutoCenter,
        };
        elo.OnValueChange = (entity) =>
        {
            eloText.Text = $"Elo: {elo.Value}";
        };
        AddToPanel(colorSelector);
        AddToPanel(elo);
        AddToPanel(new MyButton("Start", () => {
            EngineIntegration engineIntegration = new(colorSelector.WhiteSelected, elo.Value);
            engineIntegration.Start();
            StateMachine.StartGame(colorSelector.WhiteSelected, engineIntegration);
        }));
        AddToPanel(eloText);
        AddToPanel(new MyButton("Back", StateMachine.Back));
    }
}
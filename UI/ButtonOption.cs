using Chess.Core;
using Chess.UI.EngineOptions;

namespace Chess.UI;

internal class ButtonOption : EngineOption
{
    public ButtonOption(EngineIntegration.Option option) : base(option)
    {
        AdjustHeightAutomatically = false;
        MyButton button = new(option.Name, () => { OnButtonPressed?.Invoke(this, option.Name); })
        {
            Size = new(0.5f, 0f)
        };
        AddChild(button);
    }
}

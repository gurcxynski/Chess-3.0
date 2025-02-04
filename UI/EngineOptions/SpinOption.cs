using Chess.Core;
using GeonBit.UI.Entities;

namespace Chess.UI.EngineOptions;

internal class SpinOption : EngineOption
{
    public SpinOption(EngineIntegration.Option option) : base(option)
    {
        Paragraph paragraph = new(option.Name)
        {
            Anchor = Anchor.TopCenter,
            Text = $"{option.Name} ({option.Default})"
        };
        AddChild(paragraph);
        Slider slider = new(int.Parse(option.Min), int.Parse(option.Max))
        {
            Value = int.Parse(option.Default),
            OnValueChange = (e) =>
            {
                paragraph.Text = $"{option.Name} ({(e as Slider).Value})";
                OnOptionChanged?.Invoke(this, (option.Name, (e as Slider).Value.ToString()));
            }
        };
        AddChild(slider);
    }
}

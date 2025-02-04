using Chess.Core;
using GeonBit.UI.Entities;

namespace Chess.UI.EngineOptions;

internal class OptionString : EngineOption
{
    public OptionString(EngineIntegration.Option option) : base(option)
    {
        AddChild(new Paragraph(option.Name) { Anchor = Anchor.TopCenter });
        TextInput input = new()
        {
            Value = option.Default,
        };
        input.OnValueChange = (e) => { OnOptionChanged?.Invoke(this, (option.Name, input.Value)); };
        AddChild(input);
    }
}

using Chess.Core;
using GeonBit.UI.Entities;

namespace Chess.UI.EngineOptions;

internal class CheckOption : EngineOption
{
    public CheckOption(EngineIntegration.Option option) : base(option)
    {
        AddChild(new Paragraph(option.Name) { Anchor = Anchor.TopCenter });
        CheckBox checkBox = new()
        {
            Checked = option.Default == "true",
            Anchor = Anchor.BottomCenter
        };
        checkBox.RemoveChild(checkBox.TextParagraph);
        checkBox.OnValueChange = (e) => { OnOptionChanged?.Invoke(this, (option.Name, checkBox.Checked ? "true" : "false")); };
        AddChild(checkBox);
    }
}

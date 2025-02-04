using Chess.Core;
using GeonBit.UI.Entities;

namespace Chess.UI.EngineOptions;

internal class ComboOption : EngineOption
{
    public ComboOption(EngineIntegration.Option option) : base(option)
    {
        AddChild(new Paragraph(option.Name) { Anchor = Anchor.TopCenter });
        DropDown dropDown = new();
        foreach (string value in option.Values)
        {
            dropDown.AddItem(value);
        }
        dropDown.SelectedValue = option.Default;
        dropDown.OnValueChange = (e) => { OnOptionChanged?.Invoke(this, (option.Name, dropDown.SelectedValue)); };
        AddChild(dropDown);
    }
}

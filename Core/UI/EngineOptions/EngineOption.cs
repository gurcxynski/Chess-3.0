using GeonBit.UI.Entities;
using static Chess.Core.EngineIntegration;

namespace Chess.Core.UI.EngineOptions
{
    internal class EngineOption : Panel
    {
        Option Option { get; }
        public EngineOption(Option option)
        {
            Option = option;
            AddChild(new Paragraph(option.Name) { Anchor = Anchor.TopCenter });
            Size = new(0f, 0.15f);
            AdjustHeightAutomatically = true;
            Anchor = Anchor.AutoCenter;
            Entity setting = option.Type switch
            {
                OptionType.String => new TextInput() { Value = option.Default },
                OptionType.Check => new CheckBox(),
                OptionType.Button => new Button(),
                OptionType.Spin => new Slider(int.Parse(option.Min), int.Parse(option.Max)) { Value = int.Parse(option.Default) },
                OptionType.Combo => CreateDropDown(option),
                _ => null,
            };
            setting.Anchor = Anchor.AutoCenter;
            if (option.Type == OptionType.Spin)
            {
                AddChild(setting);
                AddChild(new Paragraph(option.Min) { Anchor = Anchor.BottomLeft });
                AddChild(new Paragraph(option.Max) { Anchor = Anchor.BottomRight });
            }
            else if (setting != null)
            {
                AddChild(setting);
            }
        }

        private static DropDown CreateDropDown(Option option)
        {
            DropDown dropDown = new();
            foreach (string value in option.Values)
            {
                dropDown.AddItem(value);
            }
            dropDown.SelectedValue = option.Default;
            return dropDown;
        }
    }
}

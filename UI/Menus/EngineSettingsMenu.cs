using Chess.Core;
using Chess.UI.EngineOptions;
using GeonBit.UI.Entities;
using System.Collections.Generic;
using System.Linq;
using static Chess.Core.EngineIntegration;

namespace Chess.UI.Menus;

internal class EngineSettingsMenu : Menu
{
    readonly Dictionary<string, string> changes = [];
    public EngineSettingsMenu(EngineIntegration engineIntegration, Menu returnTo) : base("Engine Settings", [])
    {
        UseRenderTarget = true;
        List<Option> options = engineIntegration.GetOptions();
        Panel scrollablePanel = new()
        {
            Size = new(0.9f, 0.8f),
            PanelOverflowBehavior = PanelOverflowBehavior.VerticalScroll
        };

        AddToPanel(new Header("Engine Settings")
        {
            Anchor = Anchor.TopCenter,
        });
        int priority = options.Count * 2;
        foreach (var option in options)
        {
            EngineOption optionObj = option.Type switch
            {
                OptionType.Button => new ButtonOption(option),
                OptionType.Spin => new SpinOption(option),
                OptionType.Check => new CheckOption(option),
                OptionType.String => new OptionString(option),
                OptionType.Combo => new ComboOption(option),
                _ => null,
            };
            if (option.Type == OptionType.Combo)
            {
                optionObj.PriorityBonus = priority;
                priority -= 2;
            }
            optionObj.OnOptionChanged += (sender, e) =>
            {
                changes[e.Item1] = e.Item2;
            };
            if (option.Type == OptionType.Button)
            {
                optionObj.OnButtonPressed += (sender, e) =>
                {
                    engineIntegration.PushOptionButton(e);
                };
            }
            scrollablePanel.AddChild(optionObj);
        }

        AddToPanel(scrollablePanel);

        AddToPanel(new MyButton("Apply", () =>
        {
            engineIntegration.SetOptions(changes.AsEnumerable());
            changes.Clear();
            Active = returnTo;
        }));
    }
}
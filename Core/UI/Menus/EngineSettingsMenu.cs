using Chess.Core.UI.EngineOptions;
using GeonBit.UI.Entities;
using System.Collections.Generic;

namespace Chess.Core.UI.Menus
{
    internal class EngineSettingsMenu : Menu
    {
        public EngineSettingsMenu() : base(new List<Entity>())
        {
            UseRenderTarget = true;
            EngineIntegration engineIntegration = new();
            List<EngineIntegration.Option> options = engineIntegration.GetOptions();

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
                var optionObj = new EngineOption(option);
                if (option.Type == EngineIntegration.OptionType.Combo)
                {
                    optionObj.PriorityBonus = priority;
                    priority -= 2;
                }
                scrollablePanel.AddChild(optionObj);
            }

            AddToPanel(scrollablePanel);

            AddToPanel(new MyButton("Back", StateMachine.Back));
        }
    }
}
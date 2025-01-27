using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Core.UI.Menus;
internal class OptionsMenu : Menu
{
    class Setting : Entity
    {
        public Setting(string name, Entity setting)
        {
            Anchor = Anchor.AutoCenter;
            Padding = Vector2.Zero;
            Size = new Vector2(0.5f, 0.2f);
            AddChild(new Label(name)
            {
                Anchor = Anchor.AutoCenter,
            });
            setting.Anchor = Anchor.AutoCenter;
            AddChild(setting);
        }
    }

    public OptionsMenu() : base([])
    {
        DropDown displayMode = new();
        DropDown resolutions = new();

        displayMode.AddItem("Windowed");
        displayMode.AddItem("Borderless Fullscreen");
        displayMode.AddItem("Fullscreen");
        displayMode.SelectedIndex = Chess.displaySettings.Fullscreen ? 2 : Chess.displaySettings.Borderless ? 1 : 0;
        displayMode.AutoSetListHeight = true;

        displayMode.OnSelectedSpecificItem("Windowed", () =>
        {
            Chess.displaySettings.SetWindowed();
            resolutions.Locked = false;
        });
        displayMode.OnSelectedSpecificItem("Borderless Fullscreen", () =>
        {
            Chess.displaySettings.SetBorderless();
            resolutions.Locked = true;
            resolutions.SelectedValue = $"{Chess.displaySettings.Width}x{Chess.displaySettings.Height}";
        });
        displayMode.OnSelectedSpecificItem("Fullscreen", () =>
        {
            Chess.displaySettings.SetFullscreen();
            resolutions.Locked = false;
        });

        var modes = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes;
        foreach (var res in modes.Reverse()) resolutions.AddItem($"{res.Width}x{res.Height}");

        resolutions.SelectedValue = $"{Chess.displaySettings.Width}x{Chess.displaySettings.Height}";
        resolutions.OnValueChange = (Entity entity) =>
        {
            int[] res = resolutions.SelectedValue.Split('x').Select(int.Parse).ToArray();
            Chess.displaySettings.SetResolution(res[0], res[1]);
        };

        AddToPanel(new Header("Options"));
        AddToPanel(new LineSpace(2));

        AddSettings(
        [
            new Setting("Display Mode", displayMode),
                    new Setting("Resolution", resolutions),
                ]);

        AddToPanel(new MyButton("Apply", () => { Chess.Instance.ApplyDisplaySettings(); }));
        AddToPanel(new MyButton("Back", StateMachine.Back));

        OnKeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter, Chess.Instance.ApplyDisplaySettings);
    }

    private void AddSettings(IEnumerable<Setting> list)
    {
        int amount = 100;
        foreach (var setting in list)
        {
            setting.PriorityBonus = amount;
            AddToPanel(setting);
            amount -= 10;
        }
    }
}

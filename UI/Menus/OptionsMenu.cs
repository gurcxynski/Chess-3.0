using Chess.Core;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.UI.Menus;
internal class OptionsMenu : Menu
{
    class Setting : Entity
    {
        public Setting(string name, Entity setting)
        {
            Anchor = Anchor.AutoCenter;
            Padding = Vector2.Zero;
            Size = new Vector2(0.5f, 0.25f);
            AddChild(new Paragraph(name)
            {
                Anchor = Anchor.AutoCenter,
            });
            setting.Anchor = Anchor.AutoCenter;
            AddChild(setting);
        }
    }

    public OptionsMenu() : base("Settings", [])
    {
        DropDown displayMode = new();
        DropDown resolutions = new();

        displayMode.AddItem("Windowed");
        displayMode.AddItem("Borderless Fullscreen");
        displayMode.AddItem("Fullscreen");
        displayMode.SelectedIndex = Core.Chess.displaySettings.Fullscreen ? 2 : Core.Chess.displaySettings.Borderless ? 1 : 0;
        displayMode.AutoSetListHeight = true;

        displayMode.OnSelectedSpecificItem("Windowed", () =>
        {
            Core.Chess.displaySettings.SetWindowed();
            resolutions.Locked = false;
        });
        displayMode.OnSelectedSpecificItem("Borderless Fullscreen", () =>
        {
            Core.Chess.displaySettings.SetBorderless();
            resolutions.Locked = true;
            resolutions.SelectedValue = $"{Core.Chess.displaySettings.Width}x{Core.Chess.displaySettings.Height}";
        });
        displayMode.OnSelectedSpecificItem("Fullscreen", () =>
        {
            Core.Chess.displaySettings.SetFullscreen();
            resolutions.Locked = false;
        });

        var modes = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes;
        foreach (var res in modes.Reverse()) if (res.Width >= 1200 && res.Height >= 800) resolutions.AddItem($"{res.Width}x{res.Height}");

        try
        {
            resolutions.SelectedValue = $"{Core.Chess.displaySettings.Width}x{Core.Chess.displaySettings.Height}";
        }
        catch
        {
            resolutions.Unselect();
        }

        resolutions.OnValueChange = (entity) =>
        {
            int[] res = resolutions.SelectedValue.Split('x').Select(int.Parse).ToArray();
            Core.Chess.displaySettings.SetResolution(res[0], res[1]);
        };

        AddToPanel(new LineSpace(2));

        AddSettings(
        [
            new Setting("Display Mode", displayMode),
            new Setting("Resolution", resolutions),
        ]);

        AddToPanel(new MyButton("Apply", Core.Chess.Instance.ApplyDisplaySettings) { Anchor = Anchor.BottomLeft });
        AddToPanel(new MyButton("Back", StateMachine.ToMenu<StartMenu>) { Anchor = Anchor.BottomRight });
    }

    private void AddSettings(List<Setting> list)
    {
        int amount = list.Count * 2;
        foreach (var setting in list)
        {
            setting.PriorityBonus = amount;
            AddToPanel(setting);
            amount -= 2;
        }
    }
}

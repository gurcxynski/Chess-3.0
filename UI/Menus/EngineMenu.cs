using Chess.Core;
using GeonBit.UI.Entities;
using System.IO;

namespace Chess.UI.Menus;

internal class EngineMenu : Menu
{
    EngineIntegration integration;
    readonly DropDown engines = new()
    {
        AutoSetListHeight = true,
    };
    public EngineMenu() : base("New Game", [])
    {
        GameCreator creator = new();
        AddToPanel(creator);
        foreach (var directory in Directory.GetDirectories("ChessEngines"))
        {
            var files = Directory.GetFiles(directory, "*.exe");
            if (files.Length == 1)
            {
                engines.AddItem(Path.GetRelativePath(Directory.GetCurrentDirectory(), files[0]));
            }
        }
        engines.OnValueChange = (entity) =>
        {
            integration = new EngineIntegration(engines.SelectedValue);
        };
        engines.SelectedIndex = 0;
        AddToPanel(engines);
        AddToPanel(new MyButton("Engine Settings", () =>
        {
            Active = new EngineSettingsMenu(integration, this);
        }));
        AddToPanel(new MyButton("Start", () =>
        {
            integration.Start();
            StateMachine.StartGame(creator.Data, integration);
        }));
        AddToPanel(new MyButton("Back", StateMachine.ToMenu<NewGameMenu>));
    }
}
using Chess.UI;
using Chess.UI.Menus;
using GeonBit.UI;
using GeonBit.UI.Entities;
using System;

namespace Chess.Core;

internal static class StateMachine
{
    internal enum GameType { Engine, Online }
    internal static void StartGame(GameCreator.GameData data, IMoveReceiver receiver) => UserInterface.Active = new PlayArea(data, receiver);
    internal static void Start() => UserInterface.Active = new StartMenu();
    internal static void QuitGame() => Environment.Exit(0);

    internal static void ToMenu<T>() where T : Menu
    {
        UserInterface.Active.AfterUpdate += (Entity e) =>
        {
            UserInterface.Active = Activator.CreateInstance<T>();
        };
    }
}
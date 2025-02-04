using Chess.UI;
using Chess.UI.Menus;
using GeonBit.UI;
using System;

namespace Chess.Core;

internal static class StateMachine
{
    internal enum GameType { Engine, Online }
    private static UserInterface PreviousState;
    internal static void StartGame(bool white, IMoveReceiver receiver) => UserInterface.Active = new PlayArea(white, receiver);
    internal static void Start() => ToMenu<StartMenu>();
    internal static void QuitGame() => Environment.Exit(0);

    internal static void ToMenu<T>() where T : Menu, new() {
        PreviousState = UserInterface.Active;
        UserInterface.Active = new T();
    }
    internal static void Back() => UserInterface.Active = PreviousState;
}
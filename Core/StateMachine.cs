using Chess.Core.UI;
using GeonBit.UI;
using System;

namespace Chess.Core;

internal static class StateMachine
{
    private static UserInterface PreviousState;
    internal static void StartGame() => UserInterface.Active = new PlayArea();

    internal static void QuitGame() => Environment.Exit(0);

    internal static void ToMenu<T>() where T : Menu, new() {
        PreviousState = UserInterface.Active;
        UserInterface.Active = new T();
    }
    internal static void Back() => UserInterface.Active = PreviousState;
}
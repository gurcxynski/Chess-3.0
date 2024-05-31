using System;
using Chess.UI;
using GeonBit.UI;

namespace Chess;

internal class StateMachine {
    private static readonly StartMenu newGame = new();

    internal static void Init()
    {
        UserInterface.Active = newGame;
    }

    internal static void StartGame(bool white)
    {
        UserInterface.Active = new ChessGame(white);
    }
    internal static void StartGame()
    {
        UserInterface.Active = new ChessGame();
    }

    internal static void QuitGame()
    {
        Chess.Instance.Exit();
    }

    internal static void ToMenu<T>() where T : Menu, new()
    {
        UserInterface.Active = new T();
    }
}
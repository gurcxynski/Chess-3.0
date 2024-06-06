using System.Numerics;
using Chess.UI;
using GeonBit.UI;
using Microsoft.VisualBasic.ApplicationServices;

namespace Chess;

internal class StateMachine {
    private static readonly StartMenu newGame = new();

    internal static void Init()
    {
        UserInterface.Active = newGame;
    }

    internal static void StartGame(ChessGame.GameType type, bool white)
    {
        Vector2 size = new(UserInterface.Active.ScreenWidth, UserInterface.Active.ScreenHeight);
        UserInterface.Active = new PlayArea(size, type, white);
    }
    internal static void StartGame(ChessGame.GameType type)
    {
        Vector2 size = new(UserInterface.Active.ScreenWidth, UserInterface.Active.ScreenHeight);
        UserInterface.Active = new PlayArea(size, type);
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
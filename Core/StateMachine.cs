using Chess.Core.UI;
using Chess.Core.UI.Menus;
using GeonBit.UI;
using System;
using System.Numerics;

namespace Chess.Core;

internal static class StateMachine
{
    private static readonly StartMenu newGame = new();

    internal static void Init() => UserInterface.Active = newGame;

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

    internal static void QuitGame() => Environment.Exit(0);

    internal static void ToMenu<T>() where T : Menu, new() => UserInterface.Active = new T();
}
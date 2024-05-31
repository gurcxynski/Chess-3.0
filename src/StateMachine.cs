using Chess.UI;
using GeonBit.UI;

namespace Chess;

internal class StateMachine {
    private static readonly StartMenu newGame = new();

    internal static void Init()
    {
        UserInterface.Active = newGame;
    }

    internal static void StartGame(ChessGame.GameType type, bool white)
    {
        UserInterface.Active = new PlayArea(type, white);
    }
    internal static void StartGame(ChessGame.GameType type)
    {
        UserInterface.Active = new PlayArea(type);
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
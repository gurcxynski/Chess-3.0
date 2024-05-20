namespace Chess;

using System;
using Chess.Core;
using Chess.UI;

internal class StateMachine {
    internal GameScreen ActiveScreen { get; private set; }
    internal void NewGame() {
        ActiveScreen = new ChessGame();
    }

    internal void Init()
    {
        NewGame();
    }
}
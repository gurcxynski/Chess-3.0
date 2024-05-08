
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input.InputListeners;

namespace Chess.Core;
internal class Scene {
    private readonly Board board = new();
    private readonly MouseListener mouseListener = new();
    private readonly KeyboardListener keyboardListener = new();

    private Piece SelectedPiece;
    internal Scene() {
        mouseListener.MouseClicked += (sender, args) => {
            Vector2 pos = new(args.Position.X / Game1.Size, args.Position.Y / Game1.Size);
            if (SelectedPiece is null) {
                Piece p = board.GetPieceAt(pos);
                if (p is not null && p.IsWhite == board.WhiteToMove) SelectedPiece = p;
                return;
            }
            Move move = new(SelectedPiece.GridPosition, pos, board);
            board.ExecuteMove(move);
            SelectedPiece = null;
        };
        keyboardListener.KeyTyped += (sender, args) => {
            board.UndoMove();
        };
    }
    internal void Update(GameTime gameTime) {
        mouseListener.Update(gameTime);
        keyboardListener.Update(gameTime);
    }
    internal void Draw(SpriteBatch spriteBatch) => board.Draw(spriteBatch);
}

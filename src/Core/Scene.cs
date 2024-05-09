using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Input.InputListeners;

namespace Chess.Core;
internal class Scene {
    private readonly Board board = new();
    private readonly MouseListener mouseListener = new();
    private readonly KeyboardListener keyboardListener = new();

    private Piece SelectedPiece;
    internal Scene() {
        mouseListener.MouseUp += (sender, args) => {
            Console.WriteLine(args.Position);
            Vector2 pos = new(args.Position.X / Game1.Size, args.Position.Y / Game1.Size);
            Piece clicked = board.GetPieceAt(pos);
            if (SelectedPiece is null && clicked is null) return;
            if (SelectedPiece is not null && (clicked is null || clicked.IsWhite != board.WhiteToMove)) {
                if (board.ExecuteMove(new Move(SelectedPiece.GridPosition, pos, board))) SelectedPiece = null;
                return;
            }
            if (clicked.IsWhite == board.WhiteToMove) SelectedPiece = clicked;

        };
        keyboardListener.KeyTyped += (sender, args) => {
            board.UndoMove();
        };
    }
    internal void Update(GameTime gameTime) {
        mouseListener.Update(gameTime);
        keyboardListener.Update(gameTime);
        
    }
    internal void Draw(SpriteBatch spriteBatch) {
        Board.DrawBoard(spriteBatch);
        if (SelectedPiece is not null) 
            spriteBatch.FillRectangle(new RectangleF(SelectedPiece.DrawPosition, new Size2(Game1.Size, Game1.Size)), new Color(0.6f, 0.6f, 0, 0.1f));
        board.DrawPieces(spriteBatch);
    }
}
